using System;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public int length;
    public int width;
    public int height;

    public Transform sectionPrefab;
    public Transform startIndicator;
    public Transform endIndicator;

    private Transform[,,] cells;
    private Stack<Point> pointStack = new Stack<Point>();
    private Point start, end;
    private System.Random random;

    private Point[] offsets = {
        new Point(1, 0, 0),
        new Point(-1, 0, 0),
        new Point(0, 1, 0),
        new Point(0, -1, 0),
        new Point(0, 0, 1),
        new Point(0, 0, -1)
    };

    // Use this for initialization
    void Start() {
        Generate();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

    void Generate() {
        // Initialize all the cells we will use
        cells = new Transform[length, height, width];
        for (int l = 0; l < length; l++) {
            for (int h = 0; h < height; h++) {
                for (int w = 0; w < width; w++) {
                    cells[l, h, w] = Instantiate(sectionPrefab, new Vector3(l, h, w), Quaternion.identity, transform);
                }
            }
        }

        // Set the starting and ending points of the maze
        start = new Point();
        end = new Point(length - 1, height - 1, width - 1);

        // Initialize randomness
        random = new System.Random();

        // Start carving
        cells[start.x, start.y, start.z].GetComponent<Cell>().modified = true;
        Carve(start);
        while (pointStack.Count != 0) {
            Carve(pointStack.Pop());
        }

        // Mark the start and end
        Instantiate(startIndicator, cells[start.x, start.y, start.z].position + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, cells[start.x, start.y, start.z]);
        Instantiate(endIndicator, cells[end.x, end.y, end.z].position + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, cells[end.x, end.y, end.z]);
    }

    void Carve(Point point) {
        if (!CanCarve(point)) {
            return;
        }

        pointStack.Push(point);

        Point next = point + offsets[random.Next(0, 6)];
        while (!IsCellOpen(next)) {
            next = point + offsets[random.Next(0, 6)];
        }

        RemoveWallsBetween(point, next);
        cells[next.x, next.y, next.z].GetComponent<Cell>().modified = true;
        Carve(next);
    }

    void RemoveWallsBetween(Point p1, Point p2) {
        Point direction = p2 - p1;

        if (direction.Equals(offsets[0])) {
            Destroy(cells[p1.x, p1.y, p1.z].GetComponent<Cell>().right);
            Destroy(cells[p2.x, p2.y, p2.z].GetComponent<Cell>().left);
        } else if (direction.Equals(offsets[1])) {
            Destroy(cells[p1.x, p1.y, p1.z].GetComponent<Cell>().left);
            Destroy(cells[p2.x, p2.y, p2.z].GetComponent<Cell>().right);
        } else if (direction.Equals(offsets[2])) {
            Destroy(cells[p1.x, p1.y, p1.z].GetComponent<Cell>().top);
            Destroy(cells[p2.x, p2.y, p2.z].GetComponent<Cell>().bottom);
        } else if (direction.Equals(offsets[3])) {
            Destroy(cells[p1.x, p1.y, p1.z].GetComponent<Cell>().bottom);
            Destroy(cells[p2.x, p2.y, p2.z].GetComponent<Cell>().top);
        } else if (direction.Equals(offsets[4])) {
            Destroy(cells[p1.x, p1.y, p1.z].GetComponent<Cell>().back);
            Destroy(cells[p2.x, p2.y, p2.z].GetComponent<Cell>().front);
        } else if (direction.Equals(offsets[5])) {
            Destroy(cells[p1.x, p1.y, p1.z].GetComponent<Cell>().front);
            Destroy(cells[p2.x, p2.y, p2.z].GetComponent<Cell>().back);
        }
    }

    bool IsCellOpen(Point point) {
        try {
            return !cells[point.x, point.y, point.z].GetComponent<Cell>().modified;
        } catch (IndexOutOfRangeException) {
            return false;
        }
    }

    bool CanCarve(Point point) {
        bool openCell = false;
        foreach (Point offset in offsets) {
            openCell = openCell || IsCellOpen(point + offset);
        }
        return openCell;
    }
}
