using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using hameluna_server.BL;
using static hameluna_server.DAL.FirestoreDBService;

namespace hameluna_server.DAL
{
    [FirestoreData]
    public class AdoptionRequestDoc
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Brand { get; set; }
        [FirestoreProperty]
        public string Price { get; set; }
    }
    public class FirestoreDBService
    {
        private readonly FirestoreDb _firestoreDb;
        private const string _collectionName = "requsts";

        public FirestoreDBService(FirestoreDb firestoreDb)
        {
            //var credentials = GoogleCredential.FromFile("path/to/serviceAccountKey.json");

            var _firestoreDb = FirestoreDb.Create("hameluna-dogbot");

            //_firestoreDb = firestoreDb;
        }


        public void TryListing()
        {
            var documentReference = _firestoreDb.Collection(_collectionName).Document("0");

            // Create a listener for the document
            var listenerRegistration = documentReference.Listen(snapshot =>
            {
                // Handle the snapshot
                Console.WriteLine($"Document updated: {snapshot.ToString}");
            });

            // Stop listening when you no longer need updates
            listenerRegistration.StopAsync();

        }

        public async Task<List<AdoptionRequest>> GetAll()
        {
            var collection = _firestoreDb.Collection(_collectionName);
            var snapshot = await collection.GetSnapshotAsync();

            var shoeDocuments = snapshot.Documents.Select(s => s.ConvertTo<AdoptionRequestDoc>()).ToList();

            return shoeDocuments.Select(ConvertDocumentToModel).ToList();
        }

        public async Task AddAsync(AdoptionRequest shoe)
        {
            var collection = _firestoreDb.Collection(_collectionName);
            var shoeDocument = ConvertModelToDocument(shoe);

            await collection.AddAsync(shoeDocument);
        }

        private static AdoptionRequest ConvertDocumentToModel(AdoptionRequestDoc shoeDocument)
        {
            return new AdoptionRequest
            {
                //Id = shoeDocument.Id,
                //Name = shoeDocument.Name,
                //Brand = shoeDocument.Brand,
                //Price = decimal.Parse(shoeDocument.Price)
            };
        }

        private static AdoptionRequestDoc ConvertModelToDocument(AdoptionRequest shoe)
        {
            return new AdoptionRequestDoc
            {
                //Id = shoe.Id,
                //Name = shoe.Name,
                //Brand = shoe.Brand,
                //Price = shoe.Price.ToString()
            };
        }
    }

}
