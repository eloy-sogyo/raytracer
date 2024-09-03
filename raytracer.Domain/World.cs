﻿namespace raytracer.Domain;

public class World 
{
    private readonly List<Shape> shapes;
    private readonly LightSource lightSource;

    public World (List<Shape> shapes, LightSource lightsource)
    {
        this.shapes = shapes;
        this.lightSource = lightsource;
    }

    public Intersection getClosestIntersection(Line line) //todo make private
    {
        Intersection closest = null;
        foreach (Shape shape in shapes)
        {
            Intersection nextIntersection = shape.intersect(line);
            if (nextIntersection != null)
            {
                if (closest == null) closest = nextIntersection;
                else
                {
                    closest = returnClosest(closest,
                        nextIntersection, line);
                }
            }
        }
        return closest;
    }

    private Intersection returnClosest(Intersection i1,
        Intersection i2, Line line)
    {
        double distance1 = (i1.getCoord() - line.getStart()).norm();
        double distance2 = (i2.getCoord() - line.getStart()).norm();

        if (distance1 < distance2) return i1;
        return i2;
    }

    public double calcBrightness(Intersection intersection) //todo make private
    {
        Vector intersectionCoord = intersection.getCoord();
        Line intersectionToLight = new Line(intersectionCoord,
            lightSource.getCoord() - intersectionCoord);

        if (getClosestIntersection(intersectionToLight) == null)
            return intersection.getShape().getDiffusionConstant(
                intersectionToLight);
        return 0.0;
    }

    public double getBrightness(Line line)
    {
        Intersection intersection = getClosestIntersection(line);
        return calcBrightness(intersection);
    }
}

