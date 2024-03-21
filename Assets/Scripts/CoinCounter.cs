
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    private int _balance;
    public int Balance { get { return _balance; } set { SetBalance(value); } }
    public string BalanceString { get => _balance.ToString();}
    

    void Start()
    {
        UpdateUI();
    }

    private void SetBalance(int balance)
    {
        _balance = balance;
        UpdateUI();
    }

    /*add or subtract from balance*/
    public int ChangeBalance(int value)
    {
        print("Balance: " + _balance.ToString());
        _balance += value;
        print("Balance after: " + _balance.ToString());
        UpdateUI();
        return _balance;
    }

    public int GetBalance()
    {
        return _balance;
    }
    
    public void UpdateUI()
    {
        GameManager.Instance.CoinValueChange(BalanceString);
    }
}
