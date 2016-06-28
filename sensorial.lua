local assert = assert
local coroutine = coroutine
local ipairs = ipairs
local os = os
local pairs = pairs
local print = print
local tonumber = tonumber

local canvas = canvas
local event = event
local http = assert(require 'http')
_ENV = nil

local api_uri = 'http://139.82.95.37:8080/api/'

local function myDrawText( myText )
  canvas:attrColor(1,1,1)
  canvas:clear()
  canvas:attrColor('yellow')
  canvas:drawText(0, 0, myText)
  canvas:flush()
end

local function actuator_request( uri )
      -- fetch URI.
      local headers = {}
      local body = ''
      local status, code, headers, body = http.put(uri, headers, body)
      if status == false then
         myDrawText('error:'..code)
      else
         myDrawText(body)
      end
end

-- attribution callbacks
event.register(
  function(evt)
    print('---------------------->ncl,attribution,start'.. evt.name)
    if evt.name == 'light_toogle' then
      http.execute(actuator_request, api_uri..'light')
    elseif evt.name == 'air_toggle' then
      http.execute(actuator_request, api_uri..'air')
    elseif evt.name == 'smell_toggle' then
      http.execute(actuator_request, api_uri..'smell')
    end
  end,
  {class='ncl', type='attribution', action='start'}
)
