﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PixelEngine.Scenes
{
    internal class SolidCubeScene : Scene
    {
        public override void Update(Keyboard keyboard, float dt)
        {
            if (keyboard.GetKeyPressed(Keys.Q))
            {
                theta_x = wrap_angle(theta_x + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.W))
            {
                theta_y = wrap_angle(theta_y + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.E))
            {
                theta_z = wrap_angle(theta_z + dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.A))
            {
                theta_x = wrap_angle(theta_x - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.S))
            {
                theta_y = wrap_angle(theta_y - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.D))
            {
                theta_z = wrap_angle(theta_z - dTheta * dt);
            }
            if (keyboard.GetKeyPressed(Keys.R))
            {
                offset_z += 2.0f * dt;
            }
            if (keyboard.GetKeyPressed(Keys.F))
            {
                offset_z -= 2.0f * dt;
            }
        }

        public override void Draw(PixelGraphics gfx)
        {
            IndexedTriangleList triangles = cube.GetTriangles().DeepCopy();

            Mat3<float> rot =
                Mat3<float>.RotationX(theta_x) *
                Mat3<float>.RotationY(theta_y) *
                Mat3<float>.RotationZ(theta_z);

            for (int i = 0; i < triangles.vertices.Count; i++)
            {
                triangles.vertices[i] *= rot;
                triangles.vertices[i] += new Vec3<float>(0.0f, 0.0f, offset_z);
            }

            for (int i = 0, end = triangles.indices.Count / 3; i < end; i++)
            {
                Vec3<float> v0 = triangles.vertices[triangles.indices[i * 3]];
                Vec3<float> v1 = triangles.vertices[triangles.indices[i * 3 + 1]];
                Vec3<float> v2 = triangles.vertices[triangles.indices[i * 3 + 2]];
                triangles.cullFlags[i] = (v1 - v0) % (v2 - v0) * v0 >= 0.0f;
            }

            for (int i = 0; i < triangles.vertices.Count; i++)
            {
                pst.Transform(triangles.vertices[i]);
            }

            for (int i = 0, end = triangles.indices.Count / 3; i < end; i++)
            {
                if (!triangles.cullFlags[i])
                {
                    gfx.DrawTriangle(
                        triangles.vertices[triangles.indices[i * 3]],
                        triangles.vertices[triangles.indices[i * 3 + 1]],
                        triangles.vertices[triangles.indices[i * 3 + 2]],
                        colors[i]);
                }
            }
        }

        PubeScreenTransformer pst = new PubeScreenTransformer();
        Cube cube = new Cube(1.0f);
        Color[] colors = new Color[12]{
		    Color.White,
		    Color.Blue,
		    Color.Cyan,
		    Color.Gray,
		    Color.Green,
		    Color.Magenta,
		    Color.LightGray,
		    Color.Red,
		    Color.Yellow,
		    Color.White,
		    Color.Blue,
		    Color.Cyan
        };
        float dTheta = (float)Math.PI;
        float offset_z = 2.0f;
        float theta_x = 0.0f;
        float theta_y = 0.0f;
        float theta_z = 0.0f;
}
}