﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IUpdateSystem : ISystem
{
    void Update(GameTime gameTime);
}
