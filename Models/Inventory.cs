﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using SleepHunterIO.Extensions;
using SleepHunterIO.IO.Process;

namespace SleepHunter.Models
{
  public sealed class Inventory : IEnumerable<InventoryItem>
  {
    static readonly string InventoryKey = @"Inventory";

    public static readonly int InventoryCount = 60;

    Player owner;
    List<InventoryItem> inventory = new List<InventoryItem>(InventoryCount);

    public Player Owner
    {
      get { return owner; }
      set { owner = value; }
    }

    public int Count { get { return inventory.Count((item) => { return !item.IsEmpty; }); } }

    public IEnumerable<string> ItemNames
    {
      get { return from i in inventory where !i.IsEmpty && !string.IsNullOrWhiteSpace(i.Name) select i.Name; }
    }

    public Inventory()
       : this(null) { }

    public Inventory(Player owner)
    {
      this.owner = owner;
      InitializeInventory();
    }

    void InitializeInventory()
    {
      inventory.Clear();

      for (int i = 0; i < inventory.Capacity; i++)
        inventory.Add(InventoryItem.MakeEmpty(i + 1));
    }

    public InventoryItem GetItem(string itemName)
    {
      itemName = itemName.Trim();

      foreach (var item in inventory)
        if (string.Equals(item.Name, itemName, StringComparison.OrdinalIgnoreCase))
          return item;

      return null;
    }

    public int FindItemSlot(string itemName)
    {
      itemName = itemName.Trim();

      foreach (var item in inventory)
        if (string.Equals(item.Name, itemName, StringComparison.OrdinalIgnoreCase))
          return item.Slot;

      return -1;
    }

    public void Update()
    {
      if (owner == null)
        throw new InvalidOperationException("Player owner is null, cannot update.");

      Update(owner.Accessor);
    }

    public void Update(ProcessMemoryAccessor accessor)
    {
      if (accessor == null)
        throw new ArgumentNullException("accessor");

      var version = this.Owner.Version;

      if (version == null)
      {
        ResetDefaults();
        return;
      }

      var inventoryVariable = version.GetVariable(InventoryKey);

      if (inventoryVariable == null)
      {
        ResetDefaults();
        return;
      }

      Debug.WriteLine($"Updating inventory (pid={accessor.ProcessId})...");

      Stream stream = null;
      try
      {
        stream = accessor.GetStream();
        using (var reader = new BinaryReader(stream, Encoding.ASCII))
        {
          stream = null;
          var inventoryPointer = inventoryVariable.DereferenceValue(reader);

          if (inventoryPointer == 0)
          {
            ResetDefaults();
            return;
          }

          reader.BaseStream.Position = inventoryPointer;

          for (int i = 0; i < inventoryVariable.Count; i++)
          {
            try
            {
              bool hasItem = reader.ReadInt16() != 0;
              ushort iconIndex = reader.ReadUInt16();
              reader.ReadByte();
              string name = reader.ReadFixedString(inventoryVariable.MaxLength);
              reader.ReadByte();

              inventory[i].IsEmpty = !hasItem;
              inventory[i].IconIndex = iconIndex;
              inventory[i].Name = name.StripNumbers();

              if (!inventory[i].IsEmpty)
                Debug.WriteLine($"Inventory slot {i + 1}: {inventory[i].Name} (icon={inventory[i].IconIndex.ToString("X")})");
            }
            catch { }
          }
        }
      }
      finally { stream?.Dispose(); }
    }

    public void ResetDefaults()
    {
      for (int i = 0; i < inventory.Capacity; i++)
      {
        inventory[i].IsEmpty = true;
        inventory[i].Name = null;
      }
    }

    #region IEnumerable Methods
    public IEnumerator<InventoryItem> GetEnumerator()
    {
      foreach (var item in inventory)
        if (!item.IsEmpty)
          yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
    #endregion
  }
}
