using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.ServerMods;

namespace Vintagestory.GameContent
{
    public class BlockEntityLiquidContainerExpanded : BlockEntityLiquidContainer
    {
        public override string InventoryClassName => "liquidcontainer";
        MeshData currentMesh;
        BlockLiquidContainerExpanded ownBlock;

        public float MeshAngle;
        public float perishMult;
        public float transitionMult;
        public float dairySpoilMult;
        public float dairyTransitionMult;
        public float fruitSpoilMult;
        public float fruitTransitionMult;
        public float grainSpoilMult;
        public float grainTransitionMult;
        public float nonutritionSpoilMult;
        public float nonutritionTransitionMult;
        public float proteinSpoilMult;
        public float proteinTransitionMult;
        public float vegetableSpoilMult;
        public float vegetableTransitionMult;
        public float unknownSpoilMult;
        public float unknownTransitionMult;

        public BlockEntityLiquidContainerExpanded()
        {
            inventory = new InventoryGeneric(1, null, null);
        }
        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            ownBlock = Block as BlockLiquidContainerExpanded;
            if (ownBlock.Attributes?["vanillaPerishMult"].Exists == true)
            {
                perishMult = ownBlock.Attributes["vanillaPerishMult"].AsFloat();
            } else
            {
                perishMult = 1f;
            }

            if (ownBlock.Attributes?["vanillaTransitionMult"].Exists == true)
            {
                transitionMult = ownBlock.Attributes["vanillaTransitionMult"].AsFloat();
            } else
            {
                transitionMult = 1.5f;
            }

            if (ownBlock.Attributes?["spoilSpeedMulByFoodCat"]["normal"].Exists == true)
            {
                dairySpoilMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["dairy"].AsFloat();
                fruitSpoilMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["fruit"].AsFloat();
                grainSpoilMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["grain"].AsFloat();
                nonutritionSpoilMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["nonutrition"].AsFloat();
                proteinSpoilMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["protein"].AsFloat();
                vegetableSpoilMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["vegetable"].AsFloat();
                unknownSpoilMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["unknown"].AsFloat();
            }
            else
            {
                dairySpoilMult = 1f;
                fruitSpoilMult = 1f;
                grainSpoilMult = 1f;
                nonutritionSpoilMult = 1f;
                proteinSpoilMult = 1f;
                vegetableSpoilMult = 1f;
                unknownSpoilMult = 1f;
            }
            if (ownBlock.Attributes?["spoilSpeedMulByFoodCat"]["normal"].Exists == true)
            {
                dairyTransitionMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["dairy"].AsFloat();
                fruitTransitionMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["fruit"].AsFloat();
                grainTransitionMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["grain"].AsFloat();
                nonutritionTransitionMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["nonutrition"].AsFloat();
                proteinTransitionMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["protein"].AsFloat();
                vegetableTransitionMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["vegetable"].AsFloat();
                unknownTransitionMult = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"]["unknown"].AsFloat();
            }
            else
            {
                dairyTransitionMult = 1.5f;
                fruitTransitionMult = 1.5f;
                grainTransitionMult = 1.5f;
                nonutritionTransitionMult = 1.5f;
                proteinTransitionMult = 1.5f;
                vegetableTransitionMult = 1.5f;
                unknownTransitionMult = 1.5f;
            }

            if (Api.Side == EnumAppSide.Client)
            {
                currentMesh = GenMesh();
                MarkDirty(true);
            }
        }
        protected override float Inventory_OnAcquireTransitionSpeed(EnumTransitionType transType, ItemStack stack, float baseMul)
        {
            if (transType == EnumTransitionType.Dry) return room?.ExitCount == 0 ? 2f : 0.5f;
            if (Api == null) return 0;
            if (stack.Item.Code.Domain == "game" || stack.Item.Code.Domain == "survival" || stack.Item.Code.Domain == "creative")
            {
                if (transType == EnumTransitionType.Perish || transType == EnumTransitionType.Cure)
                {
                    float perishRate = GetPerishRate();
                    if (transType == EnumTransitionType.Cure)
                    {
                        return GameMath.Clamp((1 - (perishRate / transitionMult)) * 3, 0, 2);
                    }

                    return baseMul * (perishRate * perishMult);
                }
            }
            else
            {
                if (ownBlock?.Attributes != null)
                {
                    float perishRate = GetPerishRate();
                    Debug.WriteLine("Base Mul = " +baseMul+"  " +"Perish Rate = "+perishRate);
                    if (transType == EnumTransitionType.Cure)
                    {
                        if (ownBlock.Attributes["transitionSpeedMulByType"]["normal"].Exists == true)
                        {
                            inventory.TransitionableSpeedMulByType = ownBlock.Attributes["transitionSpeedMulByType"]["normal"].AsObject<Dictionary<EnumTransitionType, float>>();
                            string foodcat = "?";
                            foodcat = stack?.Collectible.NutritionProps?.FoodCategory.ToString();
                            switch (foodcat)
                            {
                                case "Dairy":
                                    return GameMath.Clamp((1 - perishRate / dairyTransitionMult) * 3, 0, 2);
                                case "Fruit":
                                    return GameMath.Clamp((1 - perishRate / fruitTransitionMult) * 3, 0, 2);
                                case "Grain":
                                    return GameMath.Clamp((1 - perishRate / grainTransitionMult) * 3, 0, 2);
                                case "NoNutrition":
                                    return GameMath.Clamp((1 - perishRate / nonutritionTransitionMult) * 3, 0, 2);
                                case "Protein":
                                    return GameMath.Clamp((1 - perishRate / proteinTransitionMult) * 3, 0, 2);
                                case "Vegetable":
                                    return GameMath.Clamp((1 - perishRate / vegetableTransitionMult) * 3, 0, 2);
                                case "Unknown":
                                    return GameMath.Clamp((1 - perishRate / unknownTransitionMult) * 3, 0, 2);
                                default:
                                    return GameMath.Clamp((1 - perishRate / transitionMult) * 3, 0, 2);
                            }
                        }
                    }
                    if (ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"].Exists == true)
                    {
                        inventory.PerishableFactorByFoodCategory = ownBlock.Attributes["spoilSpeedMulByFoodCat"]["normal"].AsObject<Dictionary<EnumFoodCategory, float>>();
                        string foodcat = "?";
                        foodcat = stack?.Collectible.NutritionProps?.FoodCategory.ToString();
                        switch (foodcat)
                        {
                            case "Dairy":
                                Debug.WriteLine("Dairy Modded");
                                Debug.WriteLine("Base Mul = " + baseMul + "  " + "Perish Rate = " + perishRate + "  " + "Spoil Mult = " + dairySpoilMult);
                                return baseMul * (perishRate * dairySpoilMult);
                            case "Fruit":
                                return baseMul * (perishRate * fruitSpoilMult);
                            case "Grain":
                                return baseMul * (perishRate * grainSpoilMult);
                            case "NoNutrition":
                                return baseMul * (perishRate * nonutritionSpoilMult);
                            case "Protein":
                                return baseMul * (perishRate * proteinSpoilMult);
                            case "Vegetable":
                                return baseMul * (perishRate * vegetableSpoilMult);
                            case "Unknown":
                                return baseMul * (perishRate * unknownSpoilMult);
                            default:
                                return baseMul * (perishRate * perishMult);
                        }
                        
                    }
                }
            }
            return 1;
        }

        public override void OnBlockBroken(IPlayer byPlayer = null)
        {
            // Don't drop inventory contents
        }


        public override void OnBlockPlaced(ItemStack byItemStack = null)
        {
            base.OnBlockPlaced(byItemStack);

            if (Api.Side == EnumAppSide.Client)
            {
                currentMesh = GenMesh();
                MarkDirty(true);
            }
        }




        internal MeshData GenMesh()
        {
            if (ownBlock == null) return null;

            MeshData mesh = ownBlock.GenMesh(Api as ICoreClientAPI, GetContent(), Pos);

            if (mesh.CustomInts != null)
            {
                for (int i = 0; i < mesh.CustomInts.Count; i++)
                {
                    mesh.CustomInts.Values[i] |= 1 << 27; // Disable water wavy
                    mesh.CustomInts.Values[i] |= 1 << 26; // Enabled weak foam
                }
            }

            return mesh;
        }

        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
        {
            mesher.AddMeshData(currentMesh.Clone().Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0, MeshAngle, 0));
            return true;
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
        {
            base.FromTreeAttributes(tree, worldForResolving);

            MeshAngle = tree.GetFloat("meshAngle", MeshAngle);
            if (Api?.Side == EnumAppSide.Client)
            {
                currentMesh = GenMesh();
                MarkDirty(true);
            }
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);

            tree.SetFloat("meshAngle", MeshAngle);
        }


        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
        {
            ItemSlot slot = inventory[0];

            if (slot.Empty)
            {
                dsc.AppendLine(Lang.Get("Empty"));
            }
            else
            {
                dsc.AppendLine(Lang.Get("Contents: {0}x{1}", slot.Itemstack.StackSize, slot.Itemstack.GetName()));
            }
        }

    }
}