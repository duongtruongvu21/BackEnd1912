using DatingApp.API.DTOs;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseController
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService) {
            _memberService = memberService;
        }



        [HttpGet]
        public ActionResult<List<MemberDto>> Get()
        {
            return _memberService.GetMembers();
        }

        [HttpGet("{username}")]
        public ActionResult<MemberDto> Get(string username)
        {
            return _memberService.GetMemberByUsername(username);
        }
    }
}