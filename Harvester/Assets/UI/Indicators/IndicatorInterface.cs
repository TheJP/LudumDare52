using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(ResourceManager))]
[RequireComponent(typeof(ResourceFieldManager))]
public class IndicatorInterface : MonoBehaviour
{
    public class Indicator
    {
        public Sprite Sprite { get; }
        public Color Tint { get; }
        public System.Func<Vector2> GetPosition { get; }
        public Indicator(Sprite sprite, System.Func<Vector2> getPosition, Color? tint = null)
        {
            Sprite = sprite;
            Tint = tint ?? Color.white;
            GetPosition = getPosition;
        }
    }

    private const string ClassIndicatorOuter = "indicator-outer";
    private const string ClassIndicatorInner = "indicator-inner";

    [SerializeField]
    private UIDocument indicatorUi;

    [SerializeField]
    private float scanRadius = 30f;

    [SerializeField]
    private float scanDuration = 10f;

    private Movement player;
    private ResourceManager resourceManager;
    private ResourceFieldManager resourceFieldManager;

    private readonly Dictionary<Indicator, VisualElement> indicators = new();

    public void Start()
    {
        var playerBase = FindObjectOfType<Base>();
        player = FindObjectOfType<Movement>();
        resourceManager = GetComponent<ResourceManager>();
        resourceFieldManager = GetComponent<ResourceFieldManager>();

        indicatorUi.rootVisualElement.Clear();

        var baseSprite = playerBase.GetComponentInChildren<SpriteRenderer>();
        AddIndicator(new Indicator(baseSprite.sprite, () => playerBase.transform.position, baseSprite.color));
    }

    public void Scan()
    {
        var scanSuccess = resourceManager.UpdateStockpile(ResourceType.Research, -5);
        if (!scanSuccess) { return; }

        //var results = Physics2D.OverlapCircleAll(player.transform.position, scanRadius); // <- TODO: scan for enemies
        foreach (var field in resourceFieldManager.Fields.Where(f => (f.transform.position - player.transform.position).sqrMagnitude < scanRadius * scanRadius))
        {
            var resourceSprite = field.GetComponentInChildren<SpriteRenderer>();
            var indicator = new Indicator(resourceSprite.sprite, () => field.transform != null ? field.transform.position : player.transform.position, resourceSprite.color);
            AddIndicator(indicator);

            field.OnCollected += () => RemoveIndicator(indicator);
            StartCoroutine(TimedRemove(indicator));
        }
    }

    private IEnumerator TimedRemove(Indicator indicator)
    {
        yield return new WaitForSeconds(scanDuration);
        RemoveIndicator(indicator);
    }

    public void Update()
    {
        var height = indicatorUi.rootVisualElement.resolvedStyle.height;
        var width = indicatorUi.rootVisualElement.resolvedStyle.width;

        foreach (var indicator in indicators)
        {
            Vector2 positionWorld = indicator.Key.GetPosition();
            Vector2 position = Camera.main.WorldToViewportPoint(positionWorld);
            if (0f <= position.x && position.x <= 1f && 0 <= position.y && position.y <= 1f)
            {
                indicator.Value.visible = false;
                continue;
            }

            Vector2 playerPositionWorld = player.transform.position;
            Vector2 playerPosition = Camera.main.WorldToViewportPoint(playerPositionWorld);

            var distance = position - playerPosition;
            var direction = distance.normalized;

            indicator.Value.style.top = new StyleLength(StyleKeyword.Auto);
            indicator.Value.style.right = new StyleLength(StyleKeyword.Auto);
            indicator.Value.style.bottom = new StyleLength(StyleKeyword.Auto);
            indicator.Value.style.left = new StyleLength(StyleKeyword.Auto);

            var projectedY = (0.5f / direction.x) * direction.y;
            var projectedX = (0.5f / direction.y) * direction.x;
            var indicatorWidth = indicator.Value.resolvedStyle.width;
            var indicatorHeight = indicator.Value.resolvedStyle.height;
            if (distance.x > 0.001f && -0.5f <= projectedY && projectedY <= 0.5f)
            {
                indicator.Value.style.right = 0;
                indicator.Value.style.bottom = (height - indicatorHeight) * (projectedY + 0.5f);
            }
            else if (distance.x < -0.001f && -0.5f <= projectedY && projectedY <= 0.5f)
            {
                indicator.Value.style.left = 0;
                indicator.Value.style.top = (height - indicatorHeight) * (projectedY + 0.5f);
            }
            else if (distance.y > 0.001f && -0.5f <= projectedX && projectedX <= 0.5f)
            {
                indicator.Value.style.top = 0;
                indicator.Value.style.left = (width - indicatorWidth) * (projectedX + 0.5f);
            }
            else if (distance.y < 0.001f && -0.5f <= projectedX && projectedX <= 0.5f)
            {
                indicator.Value.style.bottom = 0;
                indicator.Value.style.right = (width - indicatorWidth) * (projectedX + 0.5f);
            }

            indicator.Value.visible = true;
        }
    }

    public void AddIndicator(Indicator indicator)
    {
        var indicatorOuter = new VisualElement();
        indicatorOuter.AddToClassList(ClassIndicatorOuter);
        indicatorOuter.pickingMode = PickingMode.Ignore;
        indicatorOuter.visible = false; // Make initially invisible until position is set.

        var indicatorInner = new VisualElement();
        indicatorInner.AddToClassList(ClassIndicatorInner);
        indicatorInner.pickingMode = PickingMode.Ignore;
        indicatorInner.style.backgroundImage = new StyleBackground(indicator.Sprite);
        indicatorInner.style.unityBackgroundImageTintColor = indicator.Tint;

        indicatorOuter.Add(indicatorInner);
        indicatorUi.rootVisualElement.Add(indicatorOuter);

        indicators.Add(indicator, indicatorOuter);
    }

    public bool RemoveIndicator(Indicator indicator)
    {
        if (!indicators.ContainsKey(indicator)) { return false; }

        var visualElement = indicators[indicator];
        indicators.Remove(indicator);
        indicatorUi.rootVisualElement.Remove(visualElement);

        return true;
    }
}
