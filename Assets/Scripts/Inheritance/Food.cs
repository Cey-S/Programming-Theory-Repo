using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Tree
{
    [SerializeField]
    private float m_ProductionSpeed = 0.5f; // private backing field
    public float ProductionSpeed
    {
        get { return m_ProductionSpeed; }
        set
        {
            if (value < 0.0f)
            {
                Debug.LogError("You can't set a negative production speed!");
            }
            else
            {
                m_ProductionSpeed = value;
            }
        }
    }

    private float m_CurrentProduction = 0.0f;

    private void Update()
    {
        if (produceItem)
        {
            ProduceItem();
        }
    }

    void ProduceItem()
    {
        if (m_CurrentProduction > 1.0f)
        {
            int amountToAdd = Mathf.FloorToInt(m_CurrentProduction);
            int leftOver = AddItem(product.id, amountToAdd);

            m_CurrentProduction = m_CurrentProduction - amountToAdd + leftOver;
        }

        if (m_CurrentProduction < 1.0f)
        {
            m_CurrentProduction += m_ProductionSpeed * Time.deltaTime;
        }
    }

    public override string GetProductionInfo()
    {
        return $"Producing at the speed of {m_ProductionSpeed}/s";
    }
}
