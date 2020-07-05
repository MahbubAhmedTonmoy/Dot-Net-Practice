﻿using MongoDB.DTO;
using MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public interface ICandidateRepository
    {
        List<Candidate> GetCandidates();
        List<Candidate> GetCandidates(List<string> email);
        Candidate GetCandidate(string email);
        void Create(Candidate cand);
        void CreateMany(List<Candidate> cand);
        void Update(Candidate cand);

        List<CandidateSearchResult> SearchCandidate(CandidateSearchPermsDTO parms);
    }
}