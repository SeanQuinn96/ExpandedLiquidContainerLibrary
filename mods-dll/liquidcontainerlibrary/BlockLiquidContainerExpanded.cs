using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent
{
    public class BlockLiquidContainerExpanded : BlockLiquidContainerTopOpened
    {
        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);

        }

        public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
        {
            bool val = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);

            if (val)
            {
                BlockEntityLiquidContainerExpanded bect = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityLiquidContainerExpanded;
                if (bect != null)
                {
                    BlockPos targetPos = blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position;
                    double dx = byPlayer.Entity.Pos.X - (targetPos.X + blockSel.HitPosition.X);
                    double dz = byPlayer.Entity.Pos.Z - (targetPos.Z + blockSel.HitPosition.Z);
                    float angleHor = (float)Math.Atan2(dx, dz);

                    float deg22dot5rad = GameMath.PIHALF / 4;
                    float roundRad = ((int)Math.Round(angleHor / deg22dot5rad)) * deg22dot5rad;
                    bect.MeshAngle = roundRad;
                }
            }

            return val;
        }
    }
}