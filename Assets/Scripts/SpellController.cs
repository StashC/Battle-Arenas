using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [Header("Customization")]
    public int _maxMana;
    public float _manaPerSecond;
    public int _manaRegenDelay;

    [Header("Spell Array")]
    public GameObject[] _spells;

    //Private variables_____________________________________________________
    private int curMana;
    private SpellAttack curSpell;
    private int spellCount;
    private float manaTimer;
  
    void Start()
    {
        curMana = _maxMana;
        spellCount = _spells.Length;
        SelectSpell(0);   
    }
    
    public void tryCastSpell() {
        if(curMana < curSpell._manaCost) {
            //play error sound / display no mana text
            Debug.Log("Not enough mana");
            return;
        }
        if(curSpell.handleAttack()) {
            //remove mana cost, update mana timer to indicate we just attacked.
            curMana -= curSpell._manaCost;
            manaTimer = _manaRegenDelay;
        }
    }

    private void SelectSpell(int index) {
        if(index >= _spells.Length || index < 0) return;
        //deselect previous spell
        if(curSpell != null) curSpell.isSelected = false;

        GameObject spell = _spells[index];
        curSpell = spell.GetComponent<SpellAttack>();
        curSpell.isSelected = true;
    }

    void Update() {
        //we want to regen mana when:
        //it has been atleast manaRegendelay Seconds since we attacked
        //we have less than max mana
        //regen mana 1 point at a time, i.e 1 mana every 1/manaPerSecond seconds
        if(curMana < _maxMana) {
            manaTimer -= Time.deltaTime;
            if(manaTimer <= 0) {
                curMana += 1;
                manaTimer = 1 / _manaPerSecond;
            }
        }
    }

}
