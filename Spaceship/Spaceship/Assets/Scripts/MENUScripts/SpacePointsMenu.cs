//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class SpacePointsMenu : MonoBehaviour{

    [SerializeField] private GameObject advisePoints;

    [SerializeField] private Button savePoints;

    [Header("levelAndPoints")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI remainPointsText;

    [Header("Damage Texts")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI damageMultiply;

    [Header("Health Texts")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthMultiply;

    [Header("Reload Texts")]
    [SerializeField] private TextMeshProUGUI reloadText;
    [SerializeField] private TextMeshProUGUI reloadMultiply;

    [Header("Velocity Texts")]
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI velocityMultiply;

    private SpaceShipStatus newStatus;
    private int remainPoints;

    private void Start() {
        savePoints.interactable = false;
    }

    private void OnEnable() {
        newStatus = GameManager.instance.status;
        remainPoints = GameManager.instance.pointsToAddStatus;
        AtualizarTextos();

        AvisarPontosRestantes();
    }

    public void PontoAdicionar(string nameStatus) {
        if (remainPoints <= 0) return;

        int pts = ShiftPress(remainPoints);

        switch (nameStatus) {
            case "damage":
                newStatus.damagePoints += pts;
                break;

            case "health":
                newStatus.healthPoints += pts;
                break;

            case "reload":
                newStatus.reloadPoints += pts;
                break;

            case "velocity":
                newStatus.velocityPoints += pts;
                break;
        }
        remainPoints -= pts;
        AtualizarTextos();
        savePoints.interactable = true;
    }

    public void PontoRemover(string nameStatus) {

        int pts = 0;

        switch (nameStatus) {
            case "damage":
                if (newStatus.damagePoints <= 0) return;
                pts = ShiftPress(newStatus.damagePoints);
                newStatus.damagePoints -= pts;
                break;

            case "health":
                if (newStatus.healthPoints <= 0) return;
                pts = ShiftPress(newStatus.healthPoints);
                newStatus.healthPoints -= pts;
                break;

            case "reload":
                if (newStatus.reloadPoints <= 0) return;
                pts = ShiftPress(newStatus.reloadPoints);
                newStatus.reloadPoints -= pts;
                break;

            case "velocity":
                if (newStatus.velocityPoints <= 0) return;
                pts = ShiftPress(newStatus.velocityPoints);
                newStatus.velocityPoints -= pts;
                break;
        }

        remainPoints += pts;
        AtualizarTextos();
        savePoints.interactable = true;
    }

    private int ShiftPress(int points) {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            if (points >= 10) return 10;
            else return points;
        } else {
            return 1;
        }
    }

    private void AtualizarTextos() {
        levelText.text = "Level: " + GameManager.instance.levelPlayer;
        remainPointsText.text = "Remain points: " + remainPoints;

        damageText.text = newStatus.damagePoints.ToString();
        healthText.text = newStatus.healthPoints.ToString();
        reloadText.text = newStatus.reloadPoints.ToString();
        velocityText.text = newStatus.velocityPoints.ToString();

        damageMultiply.text = newStatus.damageMultiply.ToString("x 0.00");
        healthMultiply.text = newStatus.healthIncreased.ToString("+ 000");
        reloadMultiply.text = newStatus.reloadMultiply.ToString("- 0.00");
        velocityMultiply.text = newStatus.velocityMultiply.ToString("x 0.00");

    }

    public void AvisarPontosRestantes() {
        if (GameManager.instance.pointsToAddStatus > 0) advisePoints.SetActive(true);
        else advisePoints.SetActive(false);
    }


    public void SaveStatus() {
        GameManager.instance.status = newStatus;
        GameManager.instance.pointsToAddStatus = remainPoints;
        newStatus = GameManager.instance.status;
        remainPoints = GameManager.instance.pointsToAddStatus;

        GameManager.instance.verificarIntegridade();
        GameManager.instance.SaveStatus();

        AtualizarTextos();

        savePoints.interactable = false;

        AvisarPontosRestantes();
    }


}