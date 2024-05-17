namespace hameluna_server.BL
{
    public class FilesDog
    {
        public Dog Dog { get; set; }
        public List<IFormFile> Files { get; set; }
        public IFormFile ProfileImg { get; set; }

        //public int InsertDog()
        //{


        //    Dog.Insert();

        //}

        //insert files(dogId,List FILES/IMAGES, type image/file)
    }
}
