﻿using AllBirds.Application.Services.CategoryServices;
using AllBirds.DTOs.CategoryDTOs;
using AllBirds.DTOs.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AllBirds.AdminDashboard.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService _categoryService)
        {
            categoryService = _categoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ResultView<List<GetAllCategoryDTO>> getAllCategoryDTOs = await categoryService.GetAllAsync();
            ViewBag.getAllCategoryDTOs = getAllCategoryDTOs.Data;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CUCategoryDTO cUCategoryDTO)
        {
            if (ModelState.IsValid)
            {
                //To put level
                //if (cUCategoryDTO.ParentCategoryId == 0)
                //{ cUCategoryDTO.Level = 0; }//جد 
                //else if (cUCategoryDTO.ParentCategoryId != 0 && cUCategoryDTO.IsParentCategory == true)
                //{
                //    cUCategoryDTO.Level = 1;//اب

                //}
                //else if (cUCategoryDTO.ParentCategoryId != 0 && cUCategoryDTO.IsParentCategory == false)
                //{
                //    cUCategoryDTO.Level = 2; //ابن

                //}
                // cUCategoryDTO.Updated = new DateTime(1, 1, 1);

                ResultView<CUCategoryDTO> resultView = await categoryService.CreateAsync(cUCategoryDTO);

                // here we need to check if the result is success and do different things accordingly
                return RedirectToAction("GetAll");
            }
            else
            {
                return View(cUCategoryDTO);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ////to make chose parent catgory
            ResultView<List<GetAllCategoryDTO>> getAllCategoryDTOs = await categoryService.GetAllAsync();
            ViewBag.getAllCategoryDTOs = getAllCategoryDTOs.Data;
            //
            ResultView<CUCategoryDTO> ResultView = await categoryService.GetOneAsync(id);

            //GetOneCategoryDTO getOneCategoryDTO = ResultView.Data;
            //to convert from GetOneCategoryDTO to  CUCategoryDTO 
            //CUCategoryDTO cUCategoryDTO = await categoryService.convert(getOneCategoryDTO);
            return View(ResultView.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CUCategoryDTO cUCategoryDTO)
        {
            if (ModelState.IsValid)
            {
                //To put level
                //if (cUCategoryDTO.ParentCategoryId == 0)
                //{ cUCategoryDTO.Level = 0; }//جد 
                //else if (cUCategoryDTO.ParentCategoryId != 0 && cUCategoryDTO.IsParentCategory == true)
                //{
                //    cUCategoryDTO.Level = 1;//اب

                //}
                //else if (cUCategoryDTO.ParentCategoryId != 0 && cUCategoryDTO.IsParentCategory == false)
                //{
                //    cUCategoryDTO.Level = 2; //ابن

                //}

                //cUCategoryDTO.Updated = DateTime.Now;
                //cUCategoryDTO.Created=

                ResultView<CUCategoryDTO> resultView = await categoryService.UpdateAsync(cUCategoryDTO);

                // here we need to check if the result is success and do different things accordingly
                return RedirectToAction("GetAll");
            }
            else
            {
                return View(cUCategoryDTO);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            ResultView<List<GetAllCategoryDTO>> getAllCategoryDTOs = await categoryService.GetAllAsync();
            return View(getAllCategoryDTOs.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ResultView<GetOneCategoryDTO> deletCategory = await categoryService.DeleteAsync(id);
            return RedirectToAction("GetAll");
        }


        public async Task<IActionResult> CategiryDetils(int id)
        {

            ResultView<CUCategoryDTO> resultView = await categoryService.GetOneAsync(id);

            if (resultView.IsSuccess)
            {
                CUCategoryDTO cUCategoryDTO = resultView.Data!;
                ResultView<CUCategoryDTO> ResultViewParentCategory = await categoryService.GetOneAsync(cUCategoryDTO.ParentCategoryId);
                ViewBag.ParentCategoryname = ResultViewParentCategory.Data?.NameEn ?? "NF";
                //Level
                //To put level
                if (cUCategoryDTO.Level == 0)
                { ViewBag.levelName = "Main Category"; }//جد 
                else if (cUCategoryDTO.Level == 1)
                {
                    ViewBag.levelName = "Sup Category";
                    cUCategoryDTO.Level = 1;//اب

                }
                else if (cUCategoryDTO.Level == 2)
                {
                    ViewBag.levelName = "Category"; //ابن

                }
                return View(cUCategoryDTO);
            }
            else
            {
                return RedirectToAction("GetAll");
            }
        }
    }
}