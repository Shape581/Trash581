using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Life;
using Life.Network;
using ModKit.Helper;
using ModKit.Internal;
using UnityEngine.Playables;
using Format = ModKit.Helper.TextFormattingHelper;
using Life.UI;
using Life.InventorySystem;
using ModKit.Utils;
using UnityEngine.Rendering;

namespace Trash581
{
    public class Main : ModKit.ModKit
    {
        public Main(IGameAPI api) : base(api) 
        {
            PluginInformations = new ModKit.Interfaces.PluginInformations(ModKit.Helper.AssemblyHelper.GetName(), "Shape581", "1.0.0");
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            AAMenu.Menu.AddProximityTabLine(PluginInformations, 1087, Format.Color("Poubelle", Format.Colors.Purple), aaMenu =>
            {
                var player = PanelHelper.ReturnPlayerFromPanel(aaMenu);
                OpenMenu(player);
            });
            AAMenu.Menu.AddProximityTabLine(PluginInformations, 1215, Format.Color("Poubelle", Format.Colors.Purple), aaMenu =>
            {
                var player = PanelHelper.ReturnPlayerFromPanel(aaMenu);
                OpenMenu(player);
            });
            AAMenu.Menu.AddProximityTabLine(PluginInformations, 1276, Format.Color("Poubelle", Format.Colors.Purple), aaMenu =>
            {
                var player = PanelHelper.ReturnPlayerFromPanel(aaMenu);
                OpenMenu(player);
            });
            AAMenu.Menu.AddProximityTabLine(PluginInformations, 1277, Format.Color("Poubelle", Format.Colors.Purple), aaMenu =>
            {
                var player = PanelHelper.ReturnPlayerFromPanel(aaMenu);
                OpenMenu(player);
            });
            AAMenu.Menu.AddProximityTabLine(PluginInformations, 1087, Format.Color("Poubelle", Format.Colors.Purple), aaMenu =>
            {
                var player = PanelHelper.ReturnPlayerFromPanel(aaMenu);
                OpenMenu(player);
            });
            Logger.LogSuccess("Poubelle581", "Intialisé !");
        }

        public void OpenMenu(Player player)
        {
            var panel = PanelHelper.Create(Format.Color("Poubelle", Format.Colors.Purple), UIPanel.PanelType.TabPrice, player, () => OpenMenu(player));
            panel.CloseButton();
            panel.AddButton(Format.Color("Jeter", Format.Colors.Success), ui => ui.SelectTab());
            panel.AddButton("Retour", ui => AAMenu.AAMenu.menu.ProximityPanel(player));
            foreach (var elements in InventoryUtils.ReturnPlayerInventory(player))
            {
                var item = ItemUtils.GetItemById(elements.Key);
                panel.AddTabLine(Format.Color(item.itemName, Format.Colors.Info), Format.Color($"Quantité : {elements.Value}", Format.Colors.Success), ItemUtils.GetIconIdByItemId(elements.Key), ui =>
                {
                    Trash(player, item, elements.Value);
                });
            }
            panel.Display();
        }

        public void Trash(Player player, Item item, int quantity)
        {
            var panel = PanelHelper.Create(Format.Color($"Jeter - {item.itemName}", Format.Colors.Info), UIPanel.PanelType.Input, player, () => Trash(player, item, quantity));
            panel.TextLines.Add($"<b>Quantité à jeter.</b>");
            panel.SetInputPlaceholder("Combien d'objet voulez vous jeter...");
            panel.CloseButton();
            panel.PreviousButton();
            panel.AddButton(Format.Color("Confirmer", Format.Colors.Success), ui =>
            {
                if (int.TryParse(panel.inputText, out int value))
                {
                    if (InventoryUtils.CheckInventoryContainsItem(player, item.id, value))
                    {
                        InventoryUtils.RemoveFromInventory(player, item.id, value);
                        player.Notify(Format.Color("Succès", Format.Colors.Success), $"Vous avez jeter {value} {item.itemName} à la Poubelle.", NotificationManager.Type.Success);
                        OpenMenu(player);
                    }
                    else
                    {
                        player.Notify(Format.Color("Erreur", Format.Colors.Error), "Vous ne possedez pas cetet quantité sur vous.", NotificationManager.Type.Error);
                        panel.Refresh();
                    }
                }
                else
                {
                    player.Notify(Format.Color("Erreur", Format.Colors.Error), "Veuillez entrée une quantité valide.", NotificationManager.Type.Error);
                    panel.Refresh();
                }
            });
            panel.AddButton(Format.Color("Tout", Format.Colors.Success), ui =>
            {
                InventoryUtils.RemoveFromInventory(player, item.id, quantity);
                player.Notify(Format.Color("Succès", Format.Colors.Success), $"Vous avez jeter {quantity} {item.itemName} à la Poubelle.", NotificationManager.Type.Success);
                OpenMenu(player);
            });
            panel.Display();
        }
    }
}
