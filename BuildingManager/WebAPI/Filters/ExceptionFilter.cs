using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IBusinessLogic.Exceptions;
using Domain.Exceptions;
using IImporter.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        try
        {
            throw context.Exception;
        }
        catch (InvalidAttributeException e)
        {
            context.Result = new JsonResult(e.Message) { StatusCode = 404 };
        }
        catch (AlreadyExistsException e)
        {
            context.Result = new JsonResult(e.Message) { StatusCode = 400 };
        }
        catch (DataAccess.Exceptions.NotFoundException e)
        {
            context.Result = new JsonResult(e.Message) { StatusCode = 404 };
        }
        catch (IBusinessLogic.Exceptions.NotFoundException e)
        {
            context.Result = new JsonResult(e.Message) { StatusCode = 404 };
        }
        catch (InconsistentDataException e)
        {
            context.Result = new JsonResult(e.Message) { StatusCode = 400 };
        }
        catch (InvalidOperationException e)
        {
            context.Result = new JsonResult(e.Message) { StatusCode = 409 };
        }
        catch (UnauthorizedException e)
        {
            context.Result = new JsonResult(e.Message) { StatusCode = 401 };
        }
        catch (DbUpdateException e)
        {
            context.Result = new JsonResult("Cannot delete elements used by others") { StatusCode = 400 };
        }
        catch (SqlException e)
        {
            context.Result = new JsonResult("Cannot delete elements used by others") { StatusCode = 400 };
        }
        catch(InvalidFormatException e)
        {
            context.Result = new JsonResult(e.Message) { StatusCode = 400 };
        }
        catch (Exception)
        {            
            context.Result = new JsonResult("We encountered some issues, try again later") { StatusCode = 500 };
        }
    }
}