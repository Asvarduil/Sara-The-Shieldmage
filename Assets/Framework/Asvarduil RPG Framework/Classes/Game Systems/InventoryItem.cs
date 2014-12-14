using System;
using System.Text;
using System.Collections.Generic;

public enum ItemType 
{
	Consumable,
	Weapon,
	Armor,
	Accessory,
	KeyItem
}

[Serializable]
public class InventoryItem : INamed
{
	#region Variables / Properties

	public string Name;
	public string Description;
	public int Quantity;
	public int Value;
	
	public ItemType ItemType;
	public List<ItemEffect> EquipmentEffects;
	public List<ItemEffect> ConsumeEffects;

	public bool IsAvailable
	{
		get { return Quantity > 0; }
	}

	public string PresentableName
	{
		get { return Name + " x" + Quantity; }
	}

	public string EquipmentBonusText
	{
		get 
		{
			string displayName = string.IsNullOrEmpty(Name) ? "(None)" : Name;
			StringBuilder bonuses = new StringBuilder(displayName);
			bonuses.Append(Environment.NewLine);

			int effectCounter = 0;
			foreach(ItemEffect effect in EquipmentEffects)
			{
				if(effectCounter > 0)
					bonuses.Append(", ");

				string sign = effect.FixedEffect >= 0 ? "+" : string.Empty;
				int multiplier = (int)((effect.ScalingEffect * 100) - 100);
				string multiplierSign = multiplier >= 0 ? "+" : string.Empty;

				bonuses.Append(effect.TargetStat);
				bonuses.Append(" ");
				bonuses.Append(sign);
				bonuses.Append(effect.FixedEffect);

				if(multiplier != 0)
				{
					bonuses.Append(" (");
					bonuses.Append(multiplierSign);
					bonuses.Append(multiplier);
					bonuses.Append("%)");
				}

				effectCounter++;
			}

			return bonuses.ToString();
		}
	}

	public string InfoText
	{
		get
		{
			StringBuilder info = new StringBuilder(Description);
			info.Append(Environment.NewLine);

			if(EquipmentEffects.Count > 0)
			{
				info.Append("On Equip: ");
			}

			for(int i = 0; i < EquipmentEffects.Count; i++)
			{
				if(i > 0)
					info.Append(", ");

				ItemEffect effect = EquipmentEffects[i];

				string sign = effect.FixedEffect >= 0 ? "+" : string.Empty;
				int multiplier = (int)((effect.ScalingEffect * 100) - 100);
				string multiplierSign = multiplier >= 0 ? "+" : string.Empty;
				
				info.Append(effect.TargetStat);
				info.Append(" ");
				info.Append(sign);
				info.Append(effect.FixedEffect);
				
				if(multiplier != 0)
				{
					info.Append(" (");
					info.Append(multiplierSign);
					info.Append(multiplier);
					info.Append("%)");
				}
			}

			if(ConsumeEffects.Count > 0)
			{
				info.Append("On Usage: ");
			}

			for(int i = 0; i < ConsumeEffects.Count; i++)
			{
				if(i > 0)
					info.Append(", ");

				ItemEffect effect = ConsumeEffects[i];

				string sign = effect.FixedEffect >= 0 ? "+" : string.Empty;
				int multiplier = (int)((effect.ScalingEffect * 100) - 100);
				string multiplierSign = multiplier >= 0 ? "+" : string.Empty;

				info.Append(effect.TargetStat);
				info.Append(" ");
				info.Append(sign);
				info.Append(effect.FixedEffect);

				if(multiplier != 0)
				{
					info.Append(" (");
					info.Append(multiplierSign);
					info.Append(multiplier);
					info.Append("%)");
				}
			}

			return info.ToString();
		}
	}

	#endregion Variables / Properties

	#region Methods

	public InventoryItem Clone()
	{
		return new InventoryItem
				   {
					   Name = Name,
					   Quantity = Quantity,
					   Value = Value,
					   ItemType = ItemType,
					   EquipmentEffects = new List<ItemEffect>(EquipmentEffects),
				       ConsumeEffects = new List<ItemEffect>(ConsumeEffects)
				   };
	}

	#endregion Methods
}
