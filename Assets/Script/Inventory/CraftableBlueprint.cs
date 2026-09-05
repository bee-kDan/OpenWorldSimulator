using System;

[Serializable]
public class CraftableBlueprint
{
    public string itemName;

    public string Req1;
    public string Req2;

    public int Req1Ammount;
    public int Req2Ammount;

    public int numOfRequirements;

    public CraftableBlueprint(
        string name,
        string req1,
        string req2,
        int req1Ammount,
        int req2Ammount,
        int NumOfReq)
    {
        itemName = name;
        Req1 = req1;
        Req2 = req2;
        Req1Ammount = req1Ammount;
        Req2Ammount = req2Ammount;
        numOfRequirements = NumOfReq;
    }
}