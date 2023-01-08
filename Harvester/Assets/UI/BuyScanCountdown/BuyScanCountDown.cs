using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuyScanCountDown : MonoBehaviour
{
    [SerializeField]
    private UIDocument buyScanCountdown;

    [SerializeField]
    private UIDocument buyInterface;

    public void Start()
    {
        var resourceManager = FindObjectOfType<ResourceManager>();
        var turretManager = FindObjectOfType<TurretManager>();

        buyInterface.rootVisualElement.visible = false;

        var buyButton = buyScanCountdown.rootVisualElement.Q<Button>("buy-button");
        buyButton.RegisterCallback<ClickEvent>(e => buyInterface.rootVisualElement.visible = !buyInterface.rootVisualElement.visible);

        var closeButton = buyInterface.rootVisualElement.Q<Button>("close-button");
        closeButton.RegisterCallback<ClickEvent>(e => buyInterface.rootVisualElement.visible = false);

        var buySmallTower = buyInterface.rootVisualElement.Q<Button>("buy-small-tower");
        buySmallTower.RegisterCallback<ClickEvent>(e =>
        {
            var stock = resourceManager.GetStockCounts();
            var canBuild = stock[ResourceType.BuildingMaterial] >= 50 && stock[ResourceType.Research] >= 50;
            if (!canBuild) { return; }

            var paid = resourceManager.UpdateStockpile(ResourceType.BuildingMaterial, -50);
            paid &= resourceManager.UpdateStockpile(ResourceType.Research, -50);

            if (!paid) { Debug.LogWarning("Could not fully pay turret even though we checked stockpiles"); return; }
            var built = turretManager.SpawnTurret(TurretType.Small);

            if (!built) { Debug.LogWarning("Paid for turret but could not build it"); return; }

            buyInterface.rootVisualElement.visible = false;
        });
    }

    public void Update()
    {

    }
}
