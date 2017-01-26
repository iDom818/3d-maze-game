using System;

public class Point {

    public int x = 0;
    public int y = 0;
    public int z = 0;

    public Point() {
    }

    public Point(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Point operator +(Point p1, Point p2) {
        return new Point(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
    }

    public static Point operator -(Point p1, Point p2) {
        return new Point(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
    }

    public override bool Equals(object obj) {
        bool rc = false;
        if (obj is Point) {
            Point p2 = obj as Point;
            rc = (x == p2.x && y == p2.y && z == p2.z);
        }
        return rc;
    }

    public override int GetHashCode() {
        String s = String.Empty;
        s += (x.ToString() + "," + y.ToString() + "," + z.ToString());
        return s.GetHashCode();
    }
}