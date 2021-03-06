﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Content;

// Next stage button is used by MoM to move between investigators and monsters
public class NextStageButton
{
    private readonly StringKey PHASE_INVESTIGATOR = new StringKey("val", "PHASE_INVESTIGATOR");
    private readonly StringKey PHASE_MYTHOS = new StringKey("val", "PHASE_MYTHOS");
    private readonly StringKey MONSTER_STEP = new StringKey("val", "MONSTER_STEP");
    private readonly StringKey HORROR_STEP = new StringKey("val", "HORROR_STEP");

    // Construct and display
    public NextStageButton()
    {
        if (Game.Get().gameType.DisplayHeroes()) return;

        Update();
    }

    public void Update()
    {
        // Clean up everything marked as 'uiphase'
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(Game.UIPHASE))
            Object.Destroy(go);

        Color bgColor = new Color(0.05f, 0, 0, 0.9f);
        StringKey phase;
        if (Game.Get().quest.phase == Quest.MoMPhase.horror)
        {
            phase = HORROR_STEP;
        }
        else if (Game.Get().quest.phase == Quest.MoMPhase.mythos)
        {
            phase = PHASE_MYTHOS;
        }
        else if (Game.Get().quest.phase == Quest.MoMPhase.monsters)
        {
            phase = MONSTER_STEP;
        }
        else
        {
            phase = PHASE_INVESTIGATOR;
            bgColor = new Color(0, 0.05f, 0, 0.9f);
        }

        TextButton tb = new TextButton(
            new Vector2(UIScaler.GetHCenter(12f), UIScaler.GetBottom(-2.5f)),new Vector2(4, 2), 
            CommonStringKeys.TAB, delegate { Next(); });
        // Untag as dialog so this isn't cleared away
        tb.ApplyTag(Game.UIPHASE);
        tb.background.GetComponent<UnityEngine.UI.Image>().color = bgColor;
        tb.SetFont(Game.Get().gameType.GetHeaderFont());

        tb = new TextButton(
            new Vector2(UIScaler.GetHCenter(-16f), UIScaler.GetBottom(-2.5f)), new Vector2(4, 2), 
            new StringKey("val", "ITEMS"), delegate { Items(); });
        // Untag as dialog so this isn't cleared away
        tb.ApplyTag(Game.UIPHASE);
        tb.background.GetComponent<UnityEngine.UI.Image>().color = bgColor;
        tb.SetFont(Game.Get().gameType.GetHeaderFont());

        tb = new TextButton(
            new Vector2(UIScaler.GetHCenter(-12f), UIScaler.GetBottom(-2.5f)), new Vector2(4, 2), 
            CommonStringKeys.LOG, delegate { Log(); });
        // Untag as dialog so this isn't cleared away
        tb.ApplyTag(Game.UIPHASE);
        tb.background.GetComponent<UnityEngine.UI.Image>().color = bgColor;
        tb.SetFont(Game.Get().gameType.GetHeaderFont());

        tb = new TextButton(
            new Vector2(UIScaler.GetHCenter(-8f), UIScaler.GetBottom(-2.5f)), new Vector2(4, 2), 
            CommonStringKeys.SET, delegate { Set(); });
        // Untag as dialog so this isn't cleared away
        tb.ApplyTag(Game.UIPHASE);
        tb.background.GetComponent<UnityEngine.UI.Image>().color = bgColor;
        tb.SetFont(Game.Get().gameType.GetHeaderFont());

        DialogBox db;
        db = new DialogBox(new Vector2(UIScaler.GetHCenter(-4f), UIScaler.GetBottom(-2.5f)), new Vector2(16, 2), phase, Color.white, bgColor);
        db.SetFont(Game.Get().gameType.GetHeaderFont());
        db.ApplyTag(Game.UIPHASE);
        db.textObj.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetMediumFont();
        db.AddBorder();
    }

    // Button pressed
    public void Next()
    {
        if (GameObject.FindGameObjectWithTag(Game.DIALOG) != null)
        {
            return;
        }

        Game game = Game.Get();

        if (game.quest.UIItemsPresent()) return;

        // Add to undo stack
        game.quest.Save();

        if (game.quest.phase == Quest.MoMPhase.monsters)
        {
            game.audioControl.PlayTrait("horror");
        }

        if (game.quest.phase == Quest.MoMPhase.horror)
        {
            game.roundControl.EndRound();
        }
        else
        {
            game.quest.log.Add(new Quest.LogEntry(new StringKey("val", "PHASE_MYTHOS").Translate()));
            game.roundControl.HeroActivated();
        }
    }

    public void Items()
    {
        if (GameObject.FindGameObjectWithTag(Game.DIALOG) != null)
        {
            return;
        }
        new InventoryWindowMoM();
    }

    public void Log()
    {
        if (GameObject.FindGameObjectWithTag(Game.DIALOG) != null)
        {
            return;
        }
        new LogWindow();
    }

    public void Set()
    {
        if (GameObject.FindGameObjectWithTag(Game.DIALOG) != null)
        {
            return;
        }
        new SetWindow();
    }
}
