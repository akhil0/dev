﻿using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Fish360Project
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FishController : ApiController
    {
        [Route("api/string/")]
        [HttpGet]
        public String GetString()
        {
            return "Hello";
        }

        [Route("api/tripTest/")]
        [HttpPost]
        public String GetTripName(TripTO trip)
        {
            return trip.name;
        }

        [Route("api/tripTest1/")]
        [HttpGet]
        public List<TripTO> GetTrip()
        {
            TripTO trip = new TripTO();
            trip.name = "Canada";
            trip.startDate = "last week";
            trip.endDate = "monday";
            trip.id = 123;
            trip.userId = 234;
            List<TripTO> trips = new List<TripTO>();
            trips.Add(trip);

            return trips;
        }






        [Route("api/fish/")]
        [HttpGet]
        public List<FishTO> GetAllFish()
        {
            using (var db = new Fish360Project.f360Entities())
            {
                var query = (from fish in db.Fish select fish);

                List<FishTO> fishes = new List<FishTO>();

                foreach (var fsh in query)
                {
                    FishTO fish = new FishTO();
                    fish.id = fsh.id;
                    fish.name = fsh.name;
                    fish.species = fsh.species;
                    fish.tripId = (int)fsh.tripId;
                    fish.weight = (double)fsh.weight;
                    fish.length = (double)fsh.length;
                    if (fsh.caughtDate != null)
                        fish.caughtDate = ((DateTime)fsh.caughtDate).ToString("yyyy-MM-dd HH:mm:ss tt");

                    fish.girth = fsh.girth;
                    fish.spot = fsh.spot;
                    fish.presentation = fsh.presentation;
                    fish.depth = fsh.depth;
                    fish.clarity = fsh.clarity;
                    fish.temperature = fsh.temperature;

                    fishes.Add(fish);
                }

                return fishes;
            }
        }

        [Route("api/trip/{id}/fish/")]
        [HttpGet]
        public List<FishTO> GetAllFishForUserId(int id)
        {
            using (var db = new Fish360Project.f360Entities())
            {
                var query = (from fish in db.Fish
                             join trip in db.Trips on fish.tripId equals trip.id
                             where trip.id == id
                             select fish);

                List<FishTO> fishes = new List<FishTO>();

                foreach (var fsh in query)
                {
                    FishTO fish = new FishTO();
                    fish.id = fsh.id;
                    fish.name = fsh.name;
                    fish.species = fsh.species;
                    fish.tripId = (int)fsh.tripId;
                    fish.weight = (double)fsh.weight;
                    fish.length = (double)fsh.length;
                    if (fsh.caughtDate != null)
                        fish.caughtDate = ((DateTime)fsh.caughtDate).ToString("yyyy-MM-dd HH:mm:ss tt");
                    
                    fish.girth = fsh.girth;
                    fish.spot = fsh.spot;
                    fish.presentation = fsh.presentation;
                    fish.depth = fsh.depth;
                    fish.clarity = fsh.clarity;
                    fish.temperature = fsh.temperature;

                    fishes.Add(fish);
                }

                return fishes;
            }
        }

        // GET api/<controller>/5
        [Route("api/fish/{id}")]
        [HttpGet]
        public FishTO GetFishForId(int id)
        {
            using (var db = new Fish360Project.f360Entities())
            {
                var fsh = (from fish in db.Fish
                             where fish.id == id
                             select fish).FirstOrDefault();

                if (fsh == null)
                    return null;

                FishTO fishTO = new FishTO();
                fishTO.id = fsh.id;
                fishTO.name = fsh.name;
                fishTO.species = fsh.species;
                fishTO.notes = fsh.notes;
                fishTO.tripId = (int)fsh.tripId;
                fishTO.weight = (double)fsh.weight;
                fishTO.length = (double)fsh.length;

                fishTO.girth = fsh.girth;
                fishTO.spot = fsh.spot;
                fishTO.presentation = fsh.presentation;
                fishTO.depth = fsh.depth;
                fishTO.clarity = fsh.clarity;
                fishTO.temperature = fsh.temperature;
                
                if (fsh.caughtDate != null)
                    fishTO.caughtDate = ((DateTime)fsh.caughtDate).ToString("yyyy-MM-dd HH:mm:ss tt");

                return fishTO;
            }
        }

        // POST api/<controller>
        [Route("api/fish/")]
        [HttpPost]
        public void Post([FromBody]FishTO fishTO)
        {
            using (var db = new Fish360Project.f360Entities())
            {
                Fish newFish = new Fish();
                newFish.name = fishTO.name;
                newFish.species = fishTO.species;
                newFish.length = fishTO.length;
                newFish.weight = fishTO.weight;
                newFish.notes = fishTO.notes;
                newFish.caughtDate = DateTime.Parse(fishTO.caughtDate);//, "yyyy-MM-dd HH:mm:ss tt", CultureInfo.InvariantCulture);
                newFish.tripId = (int)fishTO.tripId;

                newFish.girth = fishTO.girth;
                newFish.spot = fishTO.spot;
                newFish.presentation = fishTO.presentation;
                newFish.depth = fishTO.depth;
                newFish.clarity = fishTO.clarity;
                newFish.temperature = fishTO.temperature;

                db.Fish.Add(newFish);
                db.SaveChanges();
            }
        }

        // PUT api/<controller>/5
        [Route("api/fish/")]
        [HttpPut]
        public void Put([FromBody]FishTO fishTO)
        {
            using (var db = new Fish360Project.f360Entities())
            {
                var fishDB = (from fish in db.Fish
                              where fish.id == fishTO.id
                              select fish).FirstOrDefault();

                fishDB.name = fishTO.name;
                fishDB.weight = fishTO.weight;
                fishDB.notes = fishTO.notes;
                fishDB.length = fishTO.length;
                fishDB.tripId = fishTO.tripId;
                fishDB.species = fishTO.species;

                fishDB.girth = fishTO.girth;
                fishDB.spot = fishTO.spot;
                fishDB.presentation = fishTO.presentation;
                fishDB.depth = fishTO.depth;
                fishDB.clarity = fishTO.clarity;
                fishDB.temperature = fishTO.temperature;

                fishDB.caughtDate = parseDateTimeExactFromString(fishTO.caughtDate);
                db.SaveChanges();
            }
        }

        // DELETE api/<controller>/5
        [Route("api/fish/{id}")]
        [HttpDelete]
        public void Delete(int id)
        {
            using (var db = new Fish360Project.f360Entities())
            {
                var query = (from fish in db.Fish
                             where fish.id == id
                             select fish).FirstOrDefault();

                if (query != null)
                {
                    db.Fish.Attach(query);
                    db.Fish.Remove(query);
                    db.SaveChanges();
                }
            }
        }

        private DateTime parseDateTimeExactFromString(string date)
        {
            return DateTime.Parse(date);
        }
    }
}