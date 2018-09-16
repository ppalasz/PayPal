using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PayPal.Business.DAL.Models;

namespace PayPal.Business.DAL
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectContext _context;
        
        public ProjectRepository(ProjectContext context)
        {
            _context = context;
        }

       
        public Vendors GetVendors(long idProject = 0)
        {
            
            List<string> emails = (from a in _context.Vendor select a.Email).ToList();
            Vendors vendors = new Vendors()
            {
                AuthenticatedUsers = emails
            };

            return vendors;
           
        }

        public VwProject FindProject(long idProject)
        {
            VwProject vwProject = GetProject(idProject);
            if (vwProject == null)
            {
                throw new HttpStatusCodeException(404,"Project Id='" + idProject + "' does not exist");
            }
            return (vwProject);
        }

        public List<VwProject> GetProjects()
        {
            List<VwProject> vwProjects = (from a in _context.VwProject select a).ToList();

            return vwProjects;
        }

        public VwProject GetProject(long idProject)
        {
            VwProject vwProject = (from a in _context.VwProject where a.ProjectId == idProject select a).SingleOrDefault();
            
            return vwProject;
        }

        public VwProject AddNewProject(ProjectData p)
        {

            VwProject vwProject = GetProject(p.ProjectId);
            if (vwProject != null)
            {
                throw new HttpStatusCodeException(500,"Project Id='" + p.ProjectId.ToString().ToString() + "' already exists");
            }

            string status = p.StatusName;
            int idStatus = (from s in _context.Status where s.StatusName == status select s.StatusId).SingleOrDefault();
            if (idStatus == 0)
            {
                throw new HttpStatusCodeException(404, "status '" + status + "' not found");
            }

            string projectType = p.ProjectTypeName;
            int idProjectType = (from s in _context.ProjectType where s.ProjectTypeName == projectType select s.ProjectTypeId).SingleOrDefault();
            if (idProjectType == 0)
            {
                throw new HttpStatusCodeException(404, "project type '" + projectType + "' not found");
            }

            Project pr = new Project
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                CreationDate = DateTime.Parse(p.CreationDate),
                DueDate = DateTime.Parse(p.DueDate),
                ProjectTypeId = idProjectType,
                SourceLanguage = p.SourceLanguage,
                TargetLanguage = p.TargetLanguage,
                StatusId = idStatus,
                Url = p.ProjectUrl,
                Repetition = p.WordCount.Repetition,
                WordCountIce = p.WordCount.Ice,
                WordCountExact = p.WordCount.Exact,
                WordCount69 = p.WordCount.Wordcount_69_0,
                WordCount79_70 = p.WordCount.Wordcount_79_70,
                WordCount99_80 = p.WordCount.Wordcount_99_80
            };

            _context.Project.Add(pr);
            _context.SaveChanges();

            vwProject = (from a in _context.VwProject where a.ProjectId == pr.ProjectId select a).SingleOrDefault();

            return vwProject;
            
        }

        public void UpdateProject(ProjectData p)
        { 
          
            Project project = (from a in _context.Project where a.ProjectId == p.ProjectId select a).SingleOrDefault();
            if (project == null)
            {
                throw new HttpStatusCodeException(404, "Project Id='" + p.ProjectId.ToString().ToString() + "' not found");
            }

            string status = p.StatusName;
            int idStatus = (from s in _context.Status where s.StatusName == status select s.StatusId).SingleOrDefault();
            if (idStatus == 0)
            {
                throw new HttpStatusCodeException(404,"status '" + status + "' not found");
            }

            project.DueDate = DateTime.Parse(p.DueDate);
            project.StatusId = idStatus;
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

       
    }

   
}
