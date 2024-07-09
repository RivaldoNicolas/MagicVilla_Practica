using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics.Eventing.Reader;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]", Name = "GetVilla")]
    [ApiController]
    public class VallaController : ControllerBase
    {
        //inyectando logger que esta por defecto ya en .net y lo trae para mostrar
        private readonly ILogger<VallaController> _logger;
        //usar aplicacion dbContect en nuestro controlador
        private readonly AplicationDbContect _db;
        private readonly IMapper _mapper;

        //aqui lo inicializamos
        public VallaController(ILogger<VallaController> logger,AplicationDbContect db,IMapper mapper)
        {   
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }


        //prodedimiento para obtener todo los datos metodo Get
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()//metodo 
        {
            _logger.LogInformation("Obtener todas las villas");

            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            return Ok( _mapper.Map<IEnumerable<VillaDto>>(villaList));


        }
        //procedimiento para obtener datos metodo Get
        [HttpGet("id:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == 0)
            {
                _logger.LogError("Error al Traer Villa con Id"+id);
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);//usando la base de datos
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDto>(villa));
        }
        //procedimiento para Guardar mediante Metodo Post
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>>CrearVilla([FromBody] VillaCreateDto CreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            if (await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == CreateDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe!");
                BadRequest(ModelState);
            }
            if (CreateDto == null)
            {
                return BadRequest(CreateDto);

            }
            //villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //VillaStore.villaList.Add(villaDto);

            //inserta a la base de datos nuevo registro
            Villa modelo = _mapper.Map<Villa>(CreateDto);
           
            await _db.Villas.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }
        //procedimiento para eliminar usando Metodo Delete
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest(); 
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(v=>v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            //VillaStore.villaList.Remove(villa);
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        //procedimiento para actualizar usando metodo Put
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto UpdateDto)
        {
            if (UpdateDto == null || id!= UpdateDto.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;
            Villa modelo = _mapper.Map<Villa>(UpdateDto);
         
            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        //procedimiento para actualizar usando metodo Patch
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePatchVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id ==0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v=>v.Id==id);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null) return BadRequest();
          
            patchDto.ApplyTo(villaDto, ModelState); 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa modelo = _mapper.Map<Villa>(villaDto);
            
            _db.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
