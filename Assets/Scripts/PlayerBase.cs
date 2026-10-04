// Base class for the earlier OldPlayer implementation; retained for historical context.

using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour
{
	public Equipment CurrentEquipment { get { return currentEquipmentIndex == -1 ? null : equipments[currentEquipmentIndex]; } }
	
	public delegate void EquipmentChangedHandler(Equipment newEquipment, Equipment oldEquipment);
	public event EquipmentChangedHandler OnEquipmentChanged;

	protected Equipment[] equipments;
	protected int currentEquipmentIndex = -1;
	public virtual int NumGrenades { get; protected set; }

	public float Health { get; protected set; }

	public virtual void SwitchEquipment(int index)
	{
		if (index == currentEquipmentIndex)
			return;

		var oldIndex = currentEquipmentIndex;

		if (oldIndex >= 0)
			CurrentEquipment.gameObject.SetActive(false);

		currentEquipmentIndex = index;
		CurrentEquipment.gameObject.SetActive(true);
		if (OnEquipmentChanged != null)
			OnEquipmentChanged(CurrentEquipment, oldIndex < 0 ? null : equipments[oldIndex]);
	}
}
