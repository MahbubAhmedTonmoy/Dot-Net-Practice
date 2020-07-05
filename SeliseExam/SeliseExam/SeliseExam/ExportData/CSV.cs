//using Microsoft.AspNetCore.Mvc;
//using SeliseExam.Data;
//using SeliseExam.Repository;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SeliseExam.ExportData
//{
//    public class CSV
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        public CSV(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }
//        public IActionResult CsvGenerate()
//        {
//            var getData = _unitOfWork.DemoRepository.GetAllWithOutPgging().ToList();

//            var builder = new StringBuilder();
//            builder.AppendLine("Id,Name");

//            foreach (var data in getData)
//            {
//                builder.AppendLine($"{data.Id},{data.Name}");
//            }
//            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "demo.csv");
//        }
//    }
//}
