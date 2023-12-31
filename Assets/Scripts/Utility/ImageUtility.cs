﻿using System;
using System.ComponentModel;
using UnityEngine;

using static EnumUtility;
public class ImageUtility
{

    public static Texture2D DecodeBase64Image(string base64EncodedImage)
    {
        if (string.IsNullOrEmpty(base64EncodedImage)) return null;
        try
        {
            var imageBytes = Convert.FromBase64String(base64EncodedImage);
            var texture = new Texture2D(1, 1);

            texture.LoadImage(imageBytes);
            return texture;
        } catch (Exception ex) {
            
            Debug.Log(ex); 
            return null;    
        
        }


    }

    public static string EncodeBase64Image(Texture2D texture)
    {
        if (texture == null) return null;

        byte[] imageBytes = texture.EncodeToPNG();

        string base64EncodedImage = Convert.ToBase64String(imageBytes);

        return base64EncodedImage;
    }

    public static Sprite CreateSpriteFromTexture(Texture2D texture, float pixelPerUnits = 1f)
    {
        if (texture == null) return null;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelPerUnits);
    }

    public static Color ConvertHexToColor(CustomColor hexColor)
    {
        if (ColorUtility.TryParseHtmlString(GetDescription(hexColor), out var color) == true)
        {
            return color;
        } else
        {
            throw new ArgumentException("Invalid hexadecimal color string.", nameof(hexColor));
        }

    }
}

public enum CustomColor
{
    [Description("#A0C49D")]
    Green,

    [Description("#F0F0F0")]
    LightGray,

    [Description("#C8C8C8")]
    Gray
}