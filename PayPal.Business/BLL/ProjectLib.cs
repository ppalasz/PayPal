using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PayPal.Business.DAL;
using PayPal.Business.DAL.Models;

namespace PayPal.Business.BLL
{
    public class ProjectLib
    {

        private IProjectRepository Repository { get; set; }

        public ProjectLib(IProjectRepository repository)
        {
            Repository = repository;
        }

        public List<VwProject> GetProjects()
        {
            return Repository.GetProjects();
        }

        public VwProject AddNewProject(ProjectData p)
        {
            return Repository.AddNewProject(p);
        }

        public void UpdateProject(ProjectData p)
        {
             Repository.UpdateProject(p);
        }

        public VwProject FindProject(long idProject = 0)
        {
            return Repository.FindProject(idProject);
        }

        public Vendors GetVendors(long idProject = 0)
        {
            return Repository.GetVendors(idProject);
        }

    }
}
