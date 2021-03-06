﻿using MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.DTO
{
    public class GetCandidateDataDTO
    {
        public List<OrganizationPartial> SelectedCompanies { get; set; }
        public List<string> PreferredCompanies { get; set; }
        public List<string> Functions { get; set; }
        public string CommunicationLanguage { get; set; }
        public int WageRequirement { get; set; }
        public List<string> JobLocations { get; set; }
        public List<string> DesiredCompanyValues { get; set; }
        public List<string> UndesiredSkills { get; set; }
        public List<string> DesiredSkills { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Salutation { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public List<string> PersonalStrengths { get; set; }
    }
}
