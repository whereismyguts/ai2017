﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.CodeGame.CodeWars2017.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWars2017.DevKit.CSharpCgdk {
    public class Commander {

        World world;
        Move move;

        int lastActionTick = -100;

        List<Command> commands = new List<Command>();

        internal void Select (VehicleType unitType) {
            commands.Add(new SelectCommand(unitType));
            //selectedGroup = -1;
        }

        internal void Select (int id) {
            commands.Add(new SelectCommand(id));
            //selectedGroup = id;
        }


        internal void Update (World world, Move move, Player me, Dictionary<long,ActualUnit> vehicles) {
            this.move = move;
            this.world = world;
            this.me = me;

            var selected = world.VehicleUpdates.FirstOrDefault(v => v.IsSelected && vehicles.ContainsKey(v.Id));
            if (selected != null && vehicles[selected.Id].Groups.Length>0)
                selectedGroup = vehicles[selected.Id].Groups[0];
        }

        int tick = 0;

        public void Step () {
            if (me.RemainingActionCooldownTicks == 0) {
                ExecuteNextCommand();
                lastActionTick = world.TickIndex;

                Render.Update(commands);
            }

            tick++;
        }

        private void ExecuteNextCommand () {
            if (commands.Count > 0) {
                commands[0].Execute(move);
                commands.RemoveAt(0);
            }
        }

        internal void Assign (int group) {
            commands.Add(new AssignCommand(group));
        }

        Dictionary<int, Vector> lastMoveCommand = new Dictionary<int, Vector>();

        internal void Move (double x, double y, int groupId) {

            //    commands.RemoveAll(c => c is MoveCommand);

            var v = new Vector(x, y);

            //if (lastMoveCommand.ContainsKey(groupId) && lastMoveCommand[groupId].Equals(v))
            //    return;
            if (Math.Abs(x) < 1 || Math.Abs(y) < 1/*|| lastMoveX == x && lastMoveY == y*/)
                return;

            commands.RemoveAll(c => c.GroupId == groupId);

            if (selectedGroup != groupId)
                Select(groupId);


            commands.Add(new MoveCommand(x, y, groupId));

           // lastMoveCommand[groupId] = v;
        }

        private Player me;
        private int selectedGroup;
    }



    abstract class Command {
        public int GroupId { get; internal set; } = -1;

        public abstract void Execute (Move move);
    }

    //class EraseCommand: Command {
    //    public override void Execute (Move move) {
    //        move.Action = ActionType.None;
    //    }
    //}

    class AssignCommand: Command {

        public AssignCommand (int group) {
            this.GroupId = group;
        }
        public override void Execute (Move move) {
            move.Action = ActionType.Assign;
            move.Group = GroupId;
        }
    }

    class MoveCommand: Command {
        public double X { get; private set; }
        public double Y { get; private set; }

        public MoveCommand (double x, double y, int group) {
            this.X = x;
            this.Y = y;
            GroupId = group;
        }

        public override void Execute (Move move) {
            move.Action = ActionType.Move;
            move.X = X;
            move.Y = Y;
        }
    }

    class SelectCommand: Command {
        VehicleType unitTypeToSelect;
        bool typeMatters = false;
        object rectangle;

        public SelectCommand (VehicleType unitType) {
            typeMatters = true;
            this.unitTypeToSelect = unitType;
            GroupId = -1;
        }

        public SelectCommand (int id) {
            this.GroupId = id;
        }

        public override void Execute (Move move) {
            move.Action = ActionType.ClearAndSelect;

            if (GroupId > -1) {
                move.Group = GroupId;
            }

            if (typeMatters)
                move.VehicleType = unitTypeToSelect;
            if (rectangle == null) {
                move.Top = 0;
                move.Bottom = 1024;
                move.Left = 0;
                move.Right = 1024;
            }
        }
    }
}
