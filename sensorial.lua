local assert = assert
local coroutine = coroutine
local ipairs = ipairs
local os = os
local pairs = pairs
local print = print
local tonumber = tonumber

local canvas = canvas
local event = event
local http = assert (require 'http')
_ENV = nil

local uri_light = 'http://139.82.95.37:8080/api/light'

local FT_COLOR = 'yellow'
canvas:attrColor (1,1,1)
canvas:clear ()

local function actuator_request( uri )
      -- fetch URI.
      local headers = {}
      local body = ''
      local status, code, headers, body = http.put (uri, headers, body)
      if status == false then
         print (('error: %s'):format (code))
         os.exit (1)
      end

     -- print response.
     local text = ''..body
     local family, size, style = canvas:attrFont ()
     canvas:attrColor (FT_COLOR)
     canvas:drawText (0, 0, text)
     canvas:flush ()
end

http.execute (actuator_request, uri_light)
