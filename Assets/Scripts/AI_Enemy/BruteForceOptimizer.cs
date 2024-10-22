using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteForceOptimizer : MonoBehaviour
{
    public int totalGold;
    //public int maxGold;
    int unitOne = 0, unitTwo = 0, unitThree = 0, unitFour = 0, unitFive = 0;
    void Start()
    {
        //Optimize();
    }

    void Optimize(int sA, int sB, int sC, int sD, int sE, int gA, int gB, int gC, int gD, int gE, int maxGold)
    {
        int bestStrength = 0;


        // Duyệt qua tất cả các giá trị có thể cho nA, nB, nC, nD, nE
        for (int nA = 0; nA <= 30; nA++)
        {
            for (int nB = 0; nB <= 30 - nA; nB++)
            {
                for (int nC = 0; nC <= 30 - nA - nB; nC++)
                {
                    for (int nD = 0; nD <= 30 - nA - nB - nC; nD++)
                    {
                        for (int nE = 0; nE <= 30 - nA - nB - nC - nD; nE++)
                        {
                            // Tính giá trị x
                            totalGold = gA * nA + gB * nB + gC * nC + gD * nD + gE * nE;

                            // Kiểm tra điều kiện ràng buộc
                            if (totalGold <= maxGold)
                            {
                                // Tính giá trị S
                                int S = sA * nA + sB * nB + sC * nC + sD * nD + sE * nE;

                                // Cập nhật giá trị tối ưu
                                if (S > bestStrength)
                                {
                                    bestStrength = S;
                                    unitOne = nA;
                                    unitTwo = nB;
                                    unitThree = nC;
                                    unitFour = nD;
                                    unitFive = nE;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Hiển thị kết quả tối ưu
        Debug.Log("Giá trị tối ưu: Stength = " + bestStrength);
        Debug.Log("unit One: " + unitOne);
        Debug.Log("unit Two: " + unitTwo);
        Debug.Log("unit There: " + unitThree);
        Debug.Log("unitFour: " + unitFour);
        Debug.Log("unitFive: " + unitFive);
        Debug.Log("Tổng tiền: " + totalGold);
    }
}