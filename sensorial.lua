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
local user_position

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
    if evt.name == 'light_toogle' then
      http.execute(actuator_request, api_uri..'light')
    elseif(evt.name == 'air_toggle' and evt.value == 'on' and user_position == 'close') then
      local function myFunc1()
        http.execute(actuator_request, api_uri..'air')
      end
      event.timer(2000,myFunc1)
    elseif(evt.name == 'air_toggle') then
      http.execute(actuator_request, api_uri..'air')
    elseif(evt.name == 'smell_toggle' and evt.value == 'on' and user_position == 'close') then
      local function myFunc1()
        http.execute(actuator_request, api_uri..'smell')
      end
      event.timer(2000,myFunc1)
    elseif(evt.name == 'smell_toggle') then
      http.execute(actuator_request, api_uri..'smell')
    elseif evt.name == 'user_position' then
      user_position = evt.value
      myDrawText('user_position='..user_position)
    end
  end,
  {class='ncl', type='attribution', action='start'}
)
