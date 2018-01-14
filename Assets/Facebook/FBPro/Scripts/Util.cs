
using System.Collections.Generic;
using UnityEngine;
using Facebook.MiniJSON;
namespace GS
{
    public class Util : ScriptableObject
    {
        public static string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null)
        {
            string url = string.Format("/{0}/picture", facebookID);
            string query = width != null ? "&width=" + width.ToString() : "";
            query += height != null ? "&height=" + height.ToString() : "";
            query += type != null ? "&type=" + type : "";
            query += "&redirect=false";
            if (query != "") url += ("?g" + query);
            return url;
        }
        
        public static string DeserializePictureURLString(string response)
        {
            return DeserializePictureURLObject(Json.Deserialize(response));
        }
        public static string DeserializePictureURLObject(object pictureObj)
        {
            var picture = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj)["data"]);
            object urlH = null;
            if (picture.TryGetValue("url", out urlH))
            {
                return (string)urlH;
            }

            return null;
        }
    }
}
