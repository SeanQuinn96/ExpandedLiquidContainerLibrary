using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace liquidcontainerlibrary
{
    class liquidcontainerlibraryCoreSystem : ModSystem
    {

        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            api.Logger.Notification("Liquid Library Loaded!");
            api.RegisterBlockClass("BlockLiquidContainerExpanded", typeof(BlockLiquidContainerExpanded));
            api.RegisterBlockEntityClass("EntityLiquidContainerExpanded", typeof(BlockEntityLiquidContainerExpanded));
        }
    }
}
