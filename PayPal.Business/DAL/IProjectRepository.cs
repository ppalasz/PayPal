using System;
using System.Collections.Generic;
using PayPal.Business.DAL.Models;

namespace PayPal.Business.DAL
{
    public interface IProjectRepository : IDisposable
    {
        VwProject AddNewProject(ProjectData p);

        Vendors GetVendors(long projectId);

        VwProject FindProject(long idProject);

        List<VwProject> GetProjects();

        void UpdateProject(ProjectData p);
    }
}