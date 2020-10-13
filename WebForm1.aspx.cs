using Grafitiy.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Grafitiy
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public static string pathCurrent = HttpContext.Current.Server.MapPath("~");
        public static string folderName = "dataFiles";
        public static string likesJsonFileName = "grafitiesLikes.json";
        public static string usersJsonFileName = "users.json";
        public static string likesJsonPath = System.IO.Path.Combine(pathCurrent, folderName, likesJsonFileName);
        public static string usersJsonPath = System.IO.Path.Combine(pathCurrent, folderName, usersJsonFileName);
        public static string grafitiJsonPath = System.IO.Path.Combine(pathCurrent, folderName, "grafities_1.js");
        public static string propertiesJsonPath = System.IO.Path.Combine(pathCurrent, folderName, "properties.json");
        public static string legalPlaceJsonPath = System.IO.Path.Combine(pathCurrent, folderName, "LegalPlaces_2.js");
     

        protected void Page_Load(object sender, EventArgs e)
        {
            //string a = imageInput.Name;

        }



        public static void addToJsonLikesFile(int id)
        {
            string json;
  
            List<LikesEntity> items;

            using (StreamReader r = new StreamReader(likesJsonPath))
            {
                json=r.ReadToEnd();
                items=JsonConvert.DeserializeObject<List<LikesEntity>>(json);
            }

            LikesEntity likesEntity = new LikesEntity(id, likes: 0, unlikes: 0);
            items.Add(likesEntity);

             json =JsonConvert.SerializeObject(items.ToArray());

            //write string to file
            System.IO.File.WriteAllText(likesJsonPath, json);

        }
        [WebMethod]
        public static bool addLegalPlace(string name, string description, string isLegal,string coordinat)
        {

            string json;
            LegalPlacesEntity items;

            using (StreamReader r = new StreamReader(legalPlaceJsonPath))
            {
                json=r.ReadToEnd().Replace("var json_LegalPlaces_2 =", "");

                items=JsonConvert.DeserializeObject<LegalPlacesEntity>(json);
            }

            Double id = items.features.Count+1;
            string[] words = coordinat.Split(',');
            Double xcoord = Convert.ToDouble(words[1]);
            Double ycoord = Convert.ToDouble(words[0]);

            PropertiesFeaturesLegal propertiesFeaturesLegal = new PropertiesFeaturesLegal(id, "", name, description, isLegal, xcoord, ycoord);

            List<Double> coordinates = new List<double>();
            coordinates.Add(Convert.ToDouble(words[1]));
            coordinates.Add(Convert.ToDouble(words[0]));

            Geometry geometry = new Geometry(coordinates);

            FeaturesLegal features = new FeaturesLegal(propertiesFeaturesLegal, geometry);

            items.features.Add(features);


            json=JsonConvert.SerializeObject(items);
            json="var json_LegalPlaces_2 ="+json;

            System.IO.File.WriteAllText(legalPlaceJsonPath, json);

            //addToJsonLikesFile(Convert.ToInt32(id));

            return true;
        }

        private string SaveFile(HttpPostedFile file,string fileName)
        {

            string savePath = System.IO.Path.Combine(pathCurrent, "images", fileName+".jpeg");

            //string tempfileName = "";

            //if (System.IO.File.Exists(pathToCheck))
            //{
            //    int counter = 2;
            //    while (System.IO.File.Exists(pathToCheck))
            //    {
            //        tempfileName=counter.ToString()+fileName;
            //        pathToCheck=savePath+tempfileName;
            //        counter++;
            //    }

            //    fileName=tempfileName;

            //}
         
            //savePath+=fileName+".jpeg";

            FileUpload1.SaveAs(savePath);

            return "images/"+fileName+".jpeg";

        }

        protected void addGrafitti(object sender, EventArgs e)
        {
            string name = graffitiNameInput.Value;
            string description = descriptionGraffitiInput.Value;
            string coordinat = current_center.Value;

            if (name!=""&&description!=""&&coordinat!="")
            {
                string json;
                GrafitiesJsonEntity items;


                using (StreamReader r = new StreamReader(grafitiJsonPath))
                {
                    json=r.ReadToEnd().Replace("var json_grafities_1 =", "");

                    items=JsonConvert.DeserializeObject<GrafitiesJsonEntity>(json);
                }

                Double id = items.features.Count+1;

                //string g_Name = "yarden";
                //string g_Descrip = "Alien worm";
                string g_Date = "2020-08-24";
                string user_Id = "";
                string artist_Id = "";
                string image = SaveFile(FileUpload1.PostedFile, id.ToString());
                string address = "kitroni 62";

                string[] words = coordinat.Split(',');
                Double xcoord = Convert.ToDouble(words[1]);
                Double ycoord = Convert.ToDouble(words[0]);

                PropertiesFeaturesGraffiti propertiesFeatures = new PropertiesFeaturesGraffiti(id, name, description, g_Date, user_Id, artist_Id, image, address, xcoord, ycoord);



                List<Double> coordinates = new List<double>();
                coordinates.Add(Convert.ToDouble(words[1]));
                coordinates.Add(Convert.ToDouble(words[0]));

                Geometry geometry = new Geometry(coordinates);

                FeaturesGraffiti features = new FeaturesGraffiti(propertiesFeatures, geometry);

                items.features.Add(features);


                json=JsonConvert.SerializeObject(items);
                json="var json_grafities_1 ="+json;

                System.IO.File.WriteAllText(grafitiJsonPath, json);

                addToJsonLikesFile(Convert.ToInt32(id));
            }
        }


        //[WebMethod]
        //public static bool addGrafitti(string name,string description,object img,string coordinat)
        //{
            
        //    string json;
        //    GrafitiesJsonEntity items;
   

        //    using (StreamReader r = new StreamReader(grafitiJsonPath))
        //    {
        //        json=r.ReadToEnd().Replace("var json_grafities_1 =", "");

        //        items=JsonConvert.DeserializeObject<GrafitiesJsonEntity>(json);
        //    }

        //    Double id = items.features.Count+1;
        //    //string g_Name = "yarden";
        //    //string g_Descrip = "Alien worm";
        //    string g_Date = "2020-08-24";
        //    string user_Id = "";
        //    string artist_Id = "";
        //    string image = "C:\\Users\\Hadas\\Desktop\\תמונות גרפיטי תל אביב\\רחוב שלמה 61 תל אביב.JPG";
        //    string address = "kitroni 62";

        //    string[] words = coordinat.Split(',');
        //    Double xcoord = Convert.ToDouble(words[1]);
        //    Double ycoord = Convert.ToDouble(words[0]);

        //    PropertiesFeaturesGraffiti propertiesFeatures = new PropertiesFeaturesGraffiti(id, name, description, g_Date, user_Id, artist_Id, image, address, xcoord, ycoord);

    
            
        //    List<Double> coordinates = new List<double>();
        //    coordinates.Add(Convert.ToDouble(words[1]));
        //    coordinates.Add(Convert.ToDouble(words[0]));

        //    Geometry geometry = new Geometry(coordinates);

        //    FeaturesGraffiti features = new FeaturesGraffiti(propertiesFeatures, geometry);

        //    items.features.Add(features);


        //    json=JsonConvert.SerializeObject(items);
        //    json="var json_grafities_1 ="+json;

        //    System.IO.File.WriteAllText(grafitiJsonPath, json);

        //    addToJsonLikesFile(Convert.ToInt32(id));

        //    return true;
        //}
        
        [WebMethod]
        public static PropertiesLogin GetProperties()
        {
            string json;
            PropertiesLogin properties;


            using (StreamReader r = new StreamReader(propertiesJsonPath))
            {
                json=r.ReadToEnd();
                properties=JsonConvert.DeserializeObject<PropertiesLogin>(json);
            }

            List<User> users = getUsersList();

            if(users!=null)
            foreach(User user in users)
            {
                if (user.email==properties.emailUserLogin)
                {
                    properties.userLogin=user;
                }
            }
          


            return properties;
        }

        public static void changeProperties(PropertiesLogin properties)
        {
           string json=JsonConvert.SerializeObject(properties);

         
            System.IO.File.WriteAllText(propertiesJsonPath, json);
        }

        [WebMethod]
        public static Result newSubscribe(string firstName, string lastName,string email,string password,string userType)
        {
            try
            {
                string json;
                bool alreadtExist = false;
                bool sucessNewSubscribe = false;
                string message="";
                List<User> items;


                using (StreamReader r = new StreamReader(usersJsonPath))
                {
                    json=r.ReadToEnd();
                    items=JsonConvert.DeserializeObject<List<User>>(json);
                }

                if (items!=null)
                {
                    foreach (User user in items)
                    {
                        if (user.email==email)
                        {
                            alreadtExist=true;
                            message="email exist. enter new email";
                            break;
                        }
                    }
                }
               else
                    items=new List<User>();

                if (firstName==String.Empty||lastName==String.Empty||email==String.Empty||password==String.Empty||userType==String.Empty)
                {
                    message="Please fill in all the fields";
                
                }
                else 
                { 

                    if (!alreadtExist)
                    {
                        User newUser = new User(email, password, firstName, lastName, userType, isActive:true);
                        items.Add(newUser);

                        json=JsonConvert.SerializeObject(items.ToArray());
                        System.IO.File.WriteAllText(usersJsonPath, json);

                        sucessNewSubscribe=true;

                    }
                }

                return new Result(sucessNewSubscribe, message);

            }

            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public static Result login(string email,string password)
        {
            string json;
            string message = "";
            bool successLogin = true;
            List<User> items;
            User userReturn = null;

            using (StreamReader r = new StreamReader(usersJsonPath))
            {
                json=r.ReadToEnd();
                items=JsonConvert.DeserializeObject<List<User>>(json);
            }

            if (email==String.Empty|| password==String.Empty)
            {
                message="Username or password incorrect. Try again";
                successLogin=false;
            }

            if (successLogin)
            {
                foreach (User user in items)
                {
                    if (user.email==email&&user.password==password)
                    {
                        userReturn=user;
                        break;
                    }
                }

                if (userReturn==null) 
                {
                    successLogin=false;
                    message ="Username or password incorrect. Try again";
                }

                else if (!userReturn.isActive)
                {
                    message="user is not active";
                    successLogin=false;
                }

            }

            if(userReturn!=null)
                changeProperties(new PropertiesLogin(successLogin, userReturn.email));
            else
                changeProperties(new PropertiesLogin(successLogin, String.Empty));

            if (successLogin)
             return new Result(successLogin, message,userReturn);

            return new Result(successLogin, message);
        }


        [WebMethod]
        public static void logout()
        {
            changeProperties(new PropertiesLogin(false, ""));
        }

        [WebMethod]
        public static void handelActive(string email,bool toActive)
        {
            string json;
            List<User> items;


            using (StreamReader r = new StreamReader(usersJsonPath))
            {
                json=r.ReadToEnd();
                items=JsonConvert.DeserializeObject<List<User>>(json);
            }

            foreach (User user in items)
            {
                if (user.email==email)
                {
                    user.isActive=toActive;
                    break;
                }
            }

             json = JsonConvert.SerializeObject(items);


            System.IO.File.WriteAllText(usersJsonPath, json);


        }



        [WebMethod]
        public static List<User> getUsersList()
        {
            string json;
            List<User> items;


            using (StreamReader r = new StreamReader(usersJsonPath))
            {
                json=r.ReadToEnd();
                items=JsonConvert.DeserializeObject<List<User>>(json);
            }

            return items;
        }


        [WebMethod]
        public static int likeClick(int id)
        {
            string json;
            int newLike = 0;
            List<LikesEntity> items;

            using (StreamReader r = new StreamReader(likesJsonPath))
            {
                json=r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<LikesEntity>>(json);
            }

            foreach(LikesEntity jsonLikes in items)
            {
                if (jsonLikes.Id==id)
                {
                    jsonLikes.likes++;
                    newLike=jsonLikes.likes;
                }
            }


             json = JsonConvert.SerializeObject(items.ToArray());

            //write string to file
            System.IO.File.WriteAllText(likesJsonPath, json);


            return newLike;
          

        }

        [WebMethod]
        public static int unlikeClick(int id)
        {
            string json;
            int newUnLike = 0;
            List<LikesEntity> items;

            using (StreamReader r = new StreamReader(likesJsonPath))
            {
                json=r.ReadToEnd();
                items=JsonConvert.DeserializeObject<List<LikesEntity>>(json);
            }

            foreach (LikesEntity jsonLikes in items)
            {
                if (jsonLikes.Id==id)
                {
                    jsonLikes.unlikes++;
                    newUnLike=jsonLikes.unlikes;
                }
            }


            json=JsonConvert.SerializeObject(items.ToArray());

            //write string to file
            System.IO.File.WriteAllText(likesJsonPath, json);


            return newUnLike;


        }

        [WebMethod]
        public static int getLikes(int id)
        {
        
            string json;
            List<LikesEntity> items;

            using (StreamReader r = new StreamReader(likesJsonPath))
            {
                json=r.ReadToEnd();
                items=JsonConvert.DeserializeObject<List<LikesEntity>>(json);
            }

            foreach (LikesEntity jsonLikes in items)
            {
                if (jsonLikes.Id==id)
                {
                    return jsonLikes.likes;
                }
            }

            return 0;

        }

        [WebMethod]
        public static int getUnLikes(int id)
        {
            string json;
            List<LikesEntity> items;

            using (StreamReader r = new StreamReader(likesJsonPath))
            {
                json=r.ReadToEnd();
                items=JsonConvert.DeserializeObject<List<LikesEntity>>(json);
            }

            foreach (LikesEntity jsonLikes in items)
            {
                if (jsonLikes.Id==id)
                {
                    return jsonLikes.unlikes;
                }
            }




            return 0;


        }

        protected void Uploader_DataBinding(object sender, EventArgs e)
        {

        }

     
    }
}