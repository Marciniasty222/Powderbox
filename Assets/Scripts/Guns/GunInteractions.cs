using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteractions : Interactions {
    public Gun gun;
    public override void TriggerDown()
    {
        gun.TryShoot();
    }
    public override void TouchpadDown()
    {
        if(gun.magazineSlot.magazineAttached)
            gun.magazineSlot.DetachMagazine();
    }
}