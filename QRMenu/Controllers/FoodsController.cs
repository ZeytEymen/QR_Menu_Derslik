﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QRMenu.Data;
using QRMenu.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QRMenu.Controllers
{
	public class FoodsController : Controller
	{
        private readonly ApplicationContext _context;

		public FoodsController(ApplicationContext context)
		{
            _context = context;
        }

        public ActionResult Index()
        {
            IQueryable<Food> foods = _context.Foods!;
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                foods = foods.Where(f => f.StateId == 1);
            }
            ViewData["userId"] = userId;
            return View(foods.ToList());
        }

        public ActionResult Details(int id)
        {
            Food? food = _context.Foods!.Where(f => f.Id == id).Include(f => f.State).FirstOrDefault();

            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        public ViewResult Create()
        {
            HttpContext.Session.LCID
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name");
            ViewData["NameLabel"] = _context.Strings.Where(s => s.LanguageCode = HttpContext.Session.LCID && s.Id == "NameLabel").FirstOrDefault().Value;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Price,Description,StateId")] Food food)
        {
            FileStream fileStream;

            if (ModelState.IsValid)
            {
                if (food.Picture != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    food.Picture.CopyTo(memoryStream);
                    memoryStream.Flush();
                    food.Image = memoryStream.ToArray();
                    memoryStream.Close();
                }
                if (food.FileData != null)
                {
                    food.Image = Convert.FromBase64String(food.FileData);
                }
                _context.Add(food);
                _context.SaveChanges();
                fileStream = new FileStream(food.Id.ToString() + ".jpg", FileMode.CreateNew);
                
                //food.Picture.CopyTo(fileStream);
                //fileStream.Close();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name", food.StateId);
            return View(food);
        }

        public ActionResult Edit(int id)
        {
            Food? food = _context.Foods!.Find(id);

            if (food == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name", food.StateId);
            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Name,Price,Description,StateId")] Food food)
        {
            if (ModelState.IsValid)
            {
                _context.Update(food);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name", food.StateId);
            return View(food);
        }

        public ActionResult Delete(int id)
        {
            Food? food = _context.Foods!.Where(f => f.Id == id).Include(f => f.State).FirstOrDefault();

            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = _context.Foods!.Find(id)!;

            food.StateId = 0;
            _context.Foods.Update(food);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

