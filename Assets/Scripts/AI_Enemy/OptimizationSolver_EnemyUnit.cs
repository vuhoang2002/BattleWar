/*using UnityEngine;
using Google.OrTools.LinearSolver;

public class OptimizationSolver_EnemyUnit : MonoBehaviour
{
    void Start()
    {
        // Khởi tạo solver
        Solver solver = Solver.CreateSolver("SCIP");

        // Các biến quyết định (đều là số nguyên)
        var nA = solver.MakeIntVar(0, int.MaxValue, "nA");
        var nB = solver.MakeIntVar(0, int.MaxValue, "nB");
        var nC = solver.MakeIntVar(0, int.MaxValue, "nC");
        var nD = solver.MakeIntVar(0, int.MaxValue, "nD");
        var nE = solver.MakeIntVar(0, int.MaxValue, "nE");

        // Hàm mục tiêu
        var objective = solver.Objective();
        objective.SetCoefficient(nA, 5);
        objective.SetCoefficient(nB, 4);
        objective.SetCoefficient(nC, 6);
        objective.SetCoefficient(nD, 4);
        objective.SetCoefficient(nE, 7);
        objective.SetMaximization(); // Tối đa hóa S

        // Ràng buộc
        solver.Add(45 * nA + 40 * nB + 80 * nC + 75 * nD + 75 * nE <= 2000);
        solver.Add(nA + nB + nC + nD + nE <= 30);

        // Giải bài toán
        Solver.ResultStatus resultStatus = solver.Solve();

        // Kiểm tra trạng thái kết quả
        if (resultStatus == Solver.ResultStatus.OPTIMAL)
        {
            Debug.Log("Giá trị tối ưu của S: " + solver.Objective().Value());
            Debug.Log("Giá trị các biến:");
            Debug.Log("nA: " + nA.SolutionValue());
            Debug.Log("nB: " + nB.SolutionValue());
            Debug.Log("nC: " + nC.SolutionValue());
            Debug.Log("nD: " + nD.SolutionValue());
            Debug.Log("nE: " + nE.SolutionValue());
        }
        else
        {
            Debug.Log("Không tìm thấy giải pháp tối ưu.");
        }
    }
}*/