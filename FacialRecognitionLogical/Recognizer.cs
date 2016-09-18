using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FacialRecognitionLogical
{
    public class Recognizer
    {
        private IFaceServiceClient _faceApiClient;
        private string _personGroup;

        public Recognizer(string personGroup)
        {
            _personGroup = personGroup;
            _faceApiClient = new FaceServiceClient("c5f74d80f01c476792477aac43bed8c5");
        }
        public async void AddPerson(string name, Stream image)
        {
            var person = await _faceApiClient.CreatePersonAsync(_personGroup, name);
            await _faceApiClient.AddPersonFaceAsync(_personGroup, person.PersonId, image);
            await _faceApiClient.TrainPersonGroupAsync(_personGroup);
        }
        public async Task<List<Person>> GetPersons(Stream image)
        {
            List<Person> authorizedPersons = new List<Person>();
            Guid[] faceIds = await GetFaceIds(image);
            IdentifyResult[] results = await _faceApiClient.IdentifyAsync(_personGroup, faceIds);
            foreach (IdentifyResult result in results)
            {
                if (result.Candidates.Length > 0)
                {
                    authorizedPersons.AddRange(await TransformCandidatesToPersons(result.Candidates));
                }
            }
            return authorizedPersons;
        }
        public async Task<Person> GetPerson(Guid personId)
        {
            Person person = await _faceApiClient.GetPersonAsync(_personGroup, personId);
            return person;
        }
        private async Task<Person[]> TransformCandidatesToPersons(Candidate[] candidates)
        {
            List<Person> persons = new List<Person>();
            foreach (Candidate candidate in candidates)
            {
                persons.Add(await GetPerson(candidate.PersonId));
            }
            return persons.ToArray();
        }
        private async Task<Guid[]> GetFaceIds(Stream image)
        {
            List<Guid> faceIds = new List<Guid>();
            Face[] facesInImage = await GetFaces(image);
            foreach (Face face in facesInImage)
            {
                faceIds.Add(face.FaceId);
            }
            return faceIds.ToArray();
        }
        private async Task<Face[]> GetFaces(Stream image)
        {
            Face[] faces = await _faceApiClient.DetectAsync(image);
            return faces;
        }

    }
}
