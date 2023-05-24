using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LinkMethod { LeftToLeft,LeftToRight,LeftToSole,RightToRight,RightToLeft,RightToSole,SoleToLeft,SoleToRight,SoleToSole};
public class StrategyFactory 
{
    LinkStrategy strategy;
    //
    LeftToLeft leftToLeft;
    LeftToRight leftToRight;
    LeftToSole leftToSole;
    RightToLeft rightToLeft;
    RightToRight rightToRight;
    RightToSole rightToSole;
    SoleToLeft soleToLeft;
    SoleToRight soleToRight;
    SoleToSole soleToSole;

    public void Initialize()
    {
        leftToLeft = new LeftToLeft();
        leftToRight = new LeftToRight();
        leftToSole = new LeftToSole();
        rightToLeft = new RightToLeft();
        rightToRight = new RightToRight();
        rightToSole = new RightToSole();
        soleToLeft = new SoleToLeft();
        soleToRight = new SoleToRight();
        soleToSole = new SoleToSole();
    }
    public LinkStrategy ReturnStrategy(LinkMethod method)
    {
        switch (method)
        {
            case LinkMethod.LeftToLeft:
                strategy = leftToLeft;
                break;
            case LinkMethod.LeftToRight:
                strategy = leftToRight;
                break;
            case LinkMethod.LeftToSole:
                strategy = leftToSole;
                break;
            case LinkMethod.RightToLeft:
                strategy =rightToLeft;
                break;
            case LinkMethod.RightToRight:
                strategy = rightToRight;
                break;
            case LinkMethod.RightToSole:
                strategy =rightToSole ;
                break;
            case LinkMethod.SoleToLeft:
                strategy = soleToLeft;
                break;
            case LinkMethod.SoleToRight:
                strategy = soleToRight;
                break;
            case LinkMethod.SoleToSole:
                strategy = soleToSole;
                break;
        }
        return strategy;
    }
}
