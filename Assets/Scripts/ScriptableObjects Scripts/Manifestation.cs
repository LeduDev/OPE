using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManifestationEffects{ MotherLove, Heal, Obliterate }

[CreateAssetMenu(fileName ="New Manifestation", menuName ="New Manifestation")]
public class Manifestation : ScriptableObject
{
    public string manifestationName;
    public int orbCost;
    public float duration;
    public float cooldown;
    public int unlockLv;
    public ManifestationEffects manifestationEffect;
}
