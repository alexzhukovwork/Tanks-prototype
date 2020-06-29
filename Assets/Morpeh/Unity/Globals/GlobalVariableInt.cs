namespace Morpeh.Globals {
    using System.Globalization;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Globals/Variable Int")]
    public class GlobalVariableInt : BaseGlobalVariable<int> {
        protected override int Load(string serializedData) => int.Parse(serializedData, CultureInfo.InvariantCulture);

        protected override string Save() => this.value.ToString(CultureInfo.InvariantCulture);
    }
}