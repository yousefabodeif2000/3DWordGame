using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CubicleUtility
{    // --- Tolerances (tweak to fit your game's units/precision) ---

    const float REL_TOL = 0.06f;    // 6% relative tolerance
    const float ABS_TOL = 0.01f;    // 1cm absolute tolerance (example)
    const float PLANE_TOL = 0.03f;  // max distance others can be from the fitted plane
    const float CENTROID_PROJ_FACTOR = 0.20f; // apex projection allowed fraction of side length
    const float MIN_HEIGHT_FACTOR = 0.05f;    // apex must be at least this * sideLength above base
    public static int GetTotalSpheres(Task task)
    {
        return task.CurrentSpheres.red + task.CurrentSpheres.green + task.CurrentSpheres.blue;
    }


    // -----------------------
    // PUBLIC API (what you asked for)
    // -----------------------

    // Return the apex sphere (or null).
    static public Sphere GetApex(List<Sphere> spheres)
    {
        if (spheres == null || spheres.Count < 4) return null;

        // 1) If exactly one uniquely-colored sphere exists, prefer it
        var colorGroups = spheres.GroupBy(s => s.SphereColor).ToList();
        var uniqueColored = colorGroups.Where(g => g.Count() == 1).Select(g => g.First()).ToList();
        if (uniqueColored.Count == 1) return uniqueColored[0];

        // 2) Geometric method: for each candidate, fit a plane to the other points
        var candidates = new List<(Sphere sphere, float dist)>();

        foreach (var candidate in spheres)
        {
            var others = spheres.Where(x => x != candidate).ToList();
            if (others.Count < 3) continue;

            Vector3[] otherPos = others.Select(o => o.transform.position).ToArray();
            if (!FindPlaneNormal(otherPos, out Vector3 normal, out Vector3 centroid))
                continue;

            // how flat are the other points?
            float maxOtherDist = 0f;
            foreach (var p in otherPos)
            {
                float d = Mathf.Abs(Vector3.Dot(normal, p - centroid));
                if (d > maxOtherDist) maxOtherDist = d;
            }

            // candidate distance to that plane
            float candDist = Mathf.Abs(Vector3.Dot(normal, candidate.transform.position - centroid));

            // valid apex candidate if others are planar and candidate is off the plane
            if (maxOtherDist <= PLANE_TOL && candDist > PLANE_TOL)
            {
                candidates.Add((candidate, candDist));
            }
        }

        if (candidates.Count == 1)
            return candidates[0].sphere;

        if (candidates.Count > 1)
        {
            // Pick the one with the largest distance if it's significantly larger than the rest
            candidates.Sort((a, b) => b.dist.CompareTo(a.dist));
            if (candidates[0].dist > candidates[1].dist * 1.5f)
                return candidates[0].sphere;
        }

        // Not confident about unique apex
        return null;
    }

    // Accepts full list of spheres (including apex), returns true if they form a regular triangular pyramid
    static public bool IsEquilateralTriangle(List<Sphere> spheres)
    {
        if (spheres == null || spheres.Count < 4) return false;

        var apex = GetApex(spheres);
        if (apex == null) return false;

        var baseSpheres = spheres.Where(s => s != apex).ToList();
        if (baseSpheres.Count != 3) return false;

        Vector3[] p = baseSpheres.Select(s => s.transform.position).ToArray();

        // side lengths
        float d01 = Vector3.Distance(p[0], p[1]);
        float d12 = Vector3.Distance(p[1], p[2]);
        float d20 = Vector3.Distance(p[2], p[0]);

        if (!ApproximatelyEqual(d01, d12) || !ApproximatelyEqual(d12, d20)) return false;

        float side = (d01 + d12 + d20) / 3f;
        // non-degenerate check (triangle area)
        float area = Vector3.Cross(p[1] - p[0], p[2] - p[0]).magnitude * 0.5f;
        if (area <= 1e-6f) return false;

        // base plane normal and centroid
        Vector3 baseCentroid = (p[0] + p[1] + p[2]) / 3f;
        Vector3 baseNormal = Vector3.Cross(p[1] - p[0], p[2] - p[0]).normalized;

        // apex height above plane
        float apexHeight = Mathf.Abs(Vector3.Dot(apex.transform.position - baseCentroid, baseNormal));
        if (apexHeight <= Mathf.Max(ABS_TOL, side * MIN_HEIGHT_FACTOR)) return false;

        // apex projection to plane should be near centroid (so apex is "over" the base)
        Vector3 projected = ProjectPointOntoPlane(apex.transform.position, baseCentroid, baseNormal);
        if (Vector3.Distance(projected, baseCentroid) > side * CENTROID_PROJ_FACTOR) return false;

        // apex -> each base vertex distances should be approximately equal (symmetric pyramid)
        float ad0 = Vector3.Distance(apex.transform.position, p[0]);
        float ad1 = Vector3.Distance(apex.transform.position, p[1]);
        float ad2 = Vector3.Distance(apex.transform.position, p[2]);
        if (!ApproximatelyEqual(ad0, ad1) || !ApproximatelyEqual(ad1, ad2)) return false;

        return true;
    }

    // Accepts full list of spheres (including apex), returns true if they form a regular square pyramid
    static public bool IsEquilateralRectangle(List<Sphere> spheres)
    {
        if (spheres == null || spheres.Count < 5) return false;

        var apex = GetApex(spheres);
        if (apex == null) return false;

        var baseSpheres = spheres.Where(s => s != apex).ToList();
        if (baseSpheres.Count != 4) return false;

        Vector3[] p = baseSpheres.Select(s => s.transform.position).ToArray();

        // Pairwise distances (6 values), sort ascending
        float[] distances = new float[6];
        int idx = 0;
        for (int i = 0; i < 4; i++)
            for (int j = i + 1; j < 4; j++)
                distances[idx++] = Vector3.Distance(p[i], p[j]);

        Array.Sort(distances);

        // For square: first 4 are sides (equal), last 2 diagonals (equal), diag ~= side * sqrt(2)
        float side = distances[0];
        for (int i = 1; i < 4; i++)
            if (!ApproximatelyEqual(distances[i], side)) return false;

        float diag0 = distances[4], diag1 = distances[5];
        if (!ApproximatelyEqual(diag0, diag1)) return false;
        if (!ApproximatelyEqual(diag0, side * Mathf.Sqrt(2f))) return false;

        // Ensure base is roughly planar and not degenerate:
        if (!FindPlaneNormal(p, out Vector3 baseNormal, out Vector3 baseCentroid)) return false;

        // Check points are close to plane
        float maxPlaneDist = 0f;
        foreach (var v in p)
        {
            float d = Mathf.Abs(Vector3.Dot(baseNormal, v - baseCentroid));
            if (d > maxPlaneDist) maxPlaneDist = d;
        }
        if (maxPlaneDist > PLANE_TOL) return false;

        // Order points around centroid to verify edge connectivity / convexity and right angles
        var ordered = OrderPointsAroundCentroid(p, baseNormal, baseCentroid);
        if (ordered == null || ordered.Length != 4) return false;

        // Edge lengths between ordered neighbors should match 'side' and angles should be ~90 degrees
        for (int i = 0; i < 4; i++)
        {
            Vector3 a = ordered[i];
            Vector3 b = ordered[(i + 1) % 4];
            Vector3 c = ordered[(i + 2) % 4];

            float edgeLen = Vector3.Distance(a, b);
            if (!ApproximatelyEqual(edgeLen, side)) return false;

            Vector3 e1 = (b - a).normalized;
            Vector3 e2 = (c - b).normalized;
            if (Mathf.Abs(Vector3.Dot(e1, e2)) > 0.12f) // allow some slack (~acos(0.12) ~ 83° - 97°)
                return false;
        }

        // Apex checks similar to triangle:
        float apexHeight = Mathf.Abs(Vector3.Dot(apex.transform.position - baseCentroid, baseNormal));
        if (apexHeight <= Mathf.Max(ABS_TOL, side * MIN_HEIGHT_FACTOR)) return false;

        Vector3 projectedApex = ProjectPointOntoPlane(apex.transform.position, baseCentroid, baseNormal);
        if (Vector3.Distance(projectedApex, baseCentroid) > side * CENTROID_PROJ_FACTOR) return false;

        // apex-to-base-vertex distances equal
        float[] ad = baseSpheres.Select(b => Vector3.Distance(apex.transform.position, b.transform.position)).ToArray();
        for (int i = 1; i < ad.Length; i++)
            if (!ApproximatelyEqual(ad[i], ad[0])) return false;

        return true;
    }

    // -----------------------
    // Helper utilities
    // -----------------------

    // Robust approx equal: uses relative + absolute tolerance
    static bool ApproximatelyEqual(float a, float b)
    {
        float diff = Mathf.Abs(a - b);
        float rel = REL_TOL * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b));
        return diff <= Mathf.Max(rel, ABS_TOL);
    }

    // Find a plane normal and centroid for a set of points. Returns false if degenerate.
    // For small point counts this is robust (tries to pick widely separated points).
    static bool FindPlaneNormal(Vector3[] pts, out Vector3 normal, out Vector3 centroid)
    {
        normal = Vector3.zero;
        centroid = Vector3.zero;
        if (pts == null || pts.Length < 3) return false;

        foreach (var v in pts) centroid += v;
        centroid /= pts.Length;

        // Heuristic: pick the point farthest from centroid, then the point farthest from that.
        int a = 0;
        float best = -1f;
        for (int i = 0; i < pts.Length; i++)
        {
            float d = (pts[i] - centroid).sqrMagnitude;
            if (d > best) { best = d; a = i; }
        }

        int b = -1;
        best = -1f;
        for (int i = 0; i < pts.Length; i++)
        {
            if (i == a) continue;
            float d = (pts[i] - pts[a]).sqrMagnitude;
            if (d > best) { best = d; b = i; }
        }

        if (b == -1) return false;

        Vector3 v1 = pts[a] - centroid;
        Vector3 v2 = pts[b] - centroid;
        Vector3 n = Vector3.Cross(v1, v2);
        if (n.sqrMagnitude < 1e-6f)
        {
            // fallback: try any triple combination to find a non-collinear set
            for (int i = 0; i < pts.Length - 2; i++)
            {
                for (int j = i + 1; j < pts.Length - 1; j++)
                {
                    for (int k = j + 1; k < pts.Length; k++)
                    {
                        n = Vector3.Cross(pts[j] - pts[i], pts[k] - pts[i]);
                        if (n.sqrMagnitude > 1e-6f)
                        {
                            normal = n.normalized;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        normal = n.normalized;
        return true;
    }

    // Project point onto plane defined by (planePoint, planeNormal)
    static Vector3 ProjectPointOntoPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
    {
        float d = Vector3.Dot(point - planePoint, planeNormal);
        return point - d * planeNormal;
    }

    // Order points around the centroid (projected into base plane) returning an array in CCW order.
    // Returns null on failure.
    static Vector3[] OrderPointsAroundCentroid(Vector3[] points, Vector3 planeNormal, Vector3 centroid)
    {
        if (points == null || points.Length == 0) return null;

        // Build local basis (u, v) on the plane
        Vector3 u = (points[0] - centroid);
        if (u.sqrMagnitude < 1e-8f)
        {
            // pick another
            for (int i = 1; i < points.Length; i++)
            {
                if ((points[i] - centroid).sqrMagnitude > 1e-8f) { u = points[i] - centroid; break; }
            }
            if (u.sqrMagnitude < 1e-8f) return null;
        }
        u = Vector3.ProjectOnPlane(u, planeNormal).normalized;
        Vector3 v = Vector3.Cross(planeNormal, u).normalized;

        var angles = new List<(Vector3 p, float angle)>();
        foreach (var pt in points)
        {
            Vector3 rel = pt - centroid;
            float x = Vector3.Dot(rel, u);
            float y = Vector3.Dot(rel, v);
            float a = Mathf.Atan2(y, x);
            angles.Add((pt, a));
        }

        angles.Sort((a, b) => a.angle.CompareTo(b.angle));
        return angles.Select(x => x.p).ToArray();
    }
}