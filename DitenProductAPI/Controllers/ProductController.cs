using DitenProductAPI.MyEntities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DitenProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly Context _context;
        public ProductController(Context context)
        {
            this._context = context;
        }
        // POST api/<ProductController>
        [HttpPost("create")]
        public IActionResult Post([FromBody] Product product)
        {
            var token = Request.Headers["token"];

            if (token.Count == 0) { return BadRequest("token ile giriş yapınız"); }
            if(product.Name=="") { return BadRequest(" Name alanı boş olamaz"); }
           

            if (this.controlToken(token))
            {
                product.CreatedByName = this.findUserByToken(token).UserName;
                product.CreatedDate = DateTime.Now;
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok(product);
            }

            return Ok("user not found");

        }
        [HttpGet("products")]
        public IActionResult Get()
        {
            var token = Request.Headers["token"];

            if (token.Count == 0) { return BadRequest("token ile giriş yapınız"); }

            if (this.controlToken(token))
            {
                var res = _context.Products.Select(x => new { x.Name, x.Quantity, x.IsDeleted });
                return Ok(res);

            }

            return Ok("user not found");

        }
        [HttpGet("product/{id}")]
        public IActionResult GetId(int id)
        {
            var token = Request.Headers["token"];

            if (token.Count == 0) { return BadRequest("token ile giriş yapınız"); }

            if (!this.controlToken(token))
            {
                return BadRequest("token is unvalid");
            }

            var res = _context.Products.Where(x => x.Id == id).Select(x => new { x.Name, x.Quantity, x.IsDeleted }).FirstOrDefault();

            if (res == null) { return BadRequest("product not found"); }
            return Ok(res);
        }
        [HttpGet("productByName/{name}")]
        public IActionResult GetName(string name)
        {
            var token = Request.Headers["token"];

            if (token.Count == 0) { return BadRequest("token ile giriş yapınız"); }

            if (!this.controlToken(token))
            {
                return BadRequest("token is unvalid");
            }

            var res = _context.Products.Where(x => x.Name == name).Select(x => new { x.Name, x.Quantity, x.IsDeleted }).FirstOrDefault();

            if (res == null) { return BadRequest("product not found"); }
            return Ok(res);
        }
        [HttpPost("ByQuantity")]
        public IActionResult ByQuantity([FromBody] Quantity quAntity)
        {
            var token = Request.Headers["token"];

            if (token.Count == 0) { return BadRequest("token ile giriş yapınız"); }

            if (!this.controlToken(token))
            {
                return BadRequest("token is unvalid");
            }

            var res = _context.Products.Where(x => x.Quantity > quAntity.Min && x.Quantity < quAntity.Max).Select(x => new { x.Name, x.Quantity, x.IsDeleted });
            if (res == null) { return BadRequest("Bu verilen değerler arasında quantity yok"); }
            return Ok(res);
           
        }
        // PUT api/<ProductController>/5
        [HttpPut("update/{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            var token = Request.Headers["token"];

            if (token.Count == 0) { return BadRequest("token ile giriş yapınız"); }


            if (this.controlToken(token))
            {
                Product product1 = _context.Products.Find(id);

                if (product1 == null) { return NotFound("product not found"); }

                else
                {
                    if (product.IsDeleted != null) { product1.IsDeleted = product.IsDeleted; }
                    if (product.Name != null) { product1.Name = product.Name; }
                    if (product.Quantity != null) { product1.Quantity = product.Quantity; }
                    product1.UpdatedDate = DateTime.Now;
                    product1.CreatedByName = this.findUserByToken(token).UserName;
                    product1.UpdatedByName = this.findUserByToken(token).UserName;

                    _context.Products.Update(product1);
                    _context.SaveChanges();
                    return Ok(_context.Products.Find(id));

                }
            }
            return NotFound("user not found");
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var token = Request.Headers["token"];

            if (token.Count == 0) { return BadRequest("token ile giriş yapınız"); }

            if (this.controlToken(token))
            {
                Product product = _context.Products.Find(id);

                if (product == null) { return NotFound("product not found"); }

                else
                {
                    product.IsDeleted = true;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                    return Ok(_context.Products.Find(id));

                }
            }
            return NotFound("user not found");


        }


        public bool controlToken(string token)
        {
            foreach (var user in _context.Users)
            {

                if (user.ApiToken == token)
                {
                    return true;
                }
            }
            return false;
        }
        public User findUserByToken(string token)
        {
            return _context.Users.Where(x => x.ApiToken == token).FirstOrDefault();
        }
    }
}
