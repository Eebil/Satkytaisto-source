using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class Satkytaisto : PhysicsGame
{
    public override void Begin()
    {
        Level.Height = 4000;
        Level.Width = 4000;
        Level.CreateBorders();
        Level.BackgroundColor = Color.AshGray;
        Gravity = new Vector(0, -500);
        LuoPelihahmo;
        PhysicsObject pelaaja = new PhysicsObject(100, 100, Shape.Hexagon);
        Add(pelaaja);
        Camera.Follow(pelaaja);
        // luo aliohjelma tikku-ukon luomiseen


        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    public static void LuoPelihahmo()
    {
        // luo pelihahmon osat aliohjemalla, liitä osat yhteen
 
    }

    public static void LuoOsa()
    {
        //Luo suorakulmion muotoisen rakenneosan, parametreinä pituus voi vaihdella
    }
}
