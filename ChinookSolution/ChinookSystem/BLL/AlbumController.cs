using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class AlbumController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ArtistAlbums> Albums_GetArtistAblums()
        {
            using (var context = new ChinookSystemContext())
            {
                IEnumerable<ArtistAlbums> results = from x in context.Albums
                                                    select new ArtistAlbums
                                                    {
                                                        Title = x.Title,
                                                        ReleaseYear = x.ReleaseYear,
                                                        ArtistName = x.Artist.Name
                                                    };
                return results.ToList();
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ArtistAlbums> Albums_GetAlbumsForArtist(int artistid)
        {
            using (var context = new ChinookSystemContext())
            {
                IEnumerable<ArtistAlbums> results = from x in context.Albums
                                                    where x.ArtistId == artistid
                                                    select new ArtistAlbums
                                                    {
                                                        Title = x.Title,
                                                        ReleaseYear = x.ReleaseYear,
                                                        ArtistName = x.Artist.Name,
                                                        ArtistId = x.ArtistId
                                                    };
                return results.ToList();
            }
        }

        //query to return all data of the album table
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AlbumItem> Albums_List()
        {
            using (var context = new ChinookSystemContext())
            {
                IEnumerable<AlbumItem> results = from x in context.Albums
                                                 select new AlbumItem
                                                 {
                                                     Title = x.Title,
                                                     ReleaseYear = x.ReleaseYear,
                                                     ArtistId = x.ArtistId,
                                                     AlbumId = x.AlbumId,
                                                     ReleaseLabel = x.ReleaseLabel

                                                 };
                return results.ToList();
            }
        }
        //query to look up an album
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AlbumItem Albums_FindById(int albumid)
        {
            using (var context = new ChinookSystemContext())
            {
                // (...).FirstOrDefault will return either
                //    a) the first record matching the where condition
                //    b) a null value
                AlbumItem results = (from x in context.Albums
                                     where x.AlbumId == albumid
                                     select new AlbumItem
                                     {
                                         AlbumId = x.AlbumId,
                                         Title = x.Title,
                                         ReleaseYear = x.ReleaseYear,
                                         ArtistId = x.ArtistId,
                                         ReleaseLabel = x.ReleaseLabel
                                     }).FirstOrDefault();
                return results;
            }
        }

        #region Add, Update and Delete CRUD
        //add
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int Album_Add(AlbumItem item)
        {
        using (var context = new ChinookSystemContext())
            {
                //due to the fact that we have separated the handling of our entities from the data transfer between web app and class library
                // using the viewmodel classes, we MUST create an instance of the entity and move the data from the viewmodel class to the entity instance

                Album addItem = new Album
                {
                    //why no pkey set?
                    //pkey is an indetity pkey, no value is needed
                    Title = item.Title,
                    ArtistId = item.ArtistId,
                    ReleaseYear = item.ReleaseYear,
                    ReleaseLabel = item.ReleaseLabel
                };

                //staging
                //setup in local memory
                //at this point you will not have sent anything to the database
                //therefore you will not have your new pkey yet
                context.Albums.Add(addItem);
                //commit to database
                //on this command you
                //a) execute entity validation annotation
                //b) send your local memory staging to the database for execution
                //after a successful execution your entity instance will have the new pkey (indetity) value
                context.SaveChanges();

                //at this point your entity instance has the new pkey value
                return addItem.AlbumId;
            }
        }
        //update
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Album_Update(AlbumItem item)
        {
            using (var context = new ChinookSystemContext())
            {
                //due to the fact that we have separated the handling of our entities from the data transfer between web app and class library
                // using the viewmodel classes, we MUST create an instance of the entity and move the data from the viewmodel class to the entity instance

                Album updateItem = new Album
                {
                    AlbumId = item.AlbumId,
                    Title = item.Title,
                    ArtistId = item.ArtistId,
                    ReleaseYear = item.ReleaseYear,
                    ReleaseLabel = item.ReleaseLabel
                };

                //staging
                //setup in local memory

                context.Entry(updateItem).State = System.Data.Entity.EntityState.Modified;
                //commit to database
                //on this command you
                //a) execute entity validation annotation
                //b) send your local memory staging to the database for execution

                context.SaveChanges();
            }
        }
        //delete

        //when we do an ODS CRUD on the delete, the ODS sends in the entire instance record, not just the pkey value

        //overload the Album_Delete method so it recieves a whole instance then call the actual delete method passing just 
        // the pkey value to the actual delete method
        [DataObjectMethod(DataObjectMethodType.Delete,false)]
        public void Album_Delete(AlbumItem item)
        {
            Album_Delete(item.AlbumId);
        }

        public void Album_Delete(int albumid)
        {
            using (var context = new ChinookSystemContext())
            {
                //retrieve the current entity instance based on the incoming parameter
                var exists = context.Albums.Find(albumid);
                //staged the remove
                context.Albums.Remove(exists);
                //commit the remove
                context.SaveChanges();

                //a logical delete is actually an update of the instance
            }
        }
        #endregion
    }
}
