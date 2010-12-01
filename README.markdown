A parser designed to generate [yUML](http://www.yuml.me) URLs from YAML files.

Currently only supports what I have needed it to support, as I need to add more things its support will grow.

See LICENSE for the licensing info.

Current YAML format:
    
    - class:
        name: <name>
        bases:
          - <superclasses>
          - <or interfaces>
        compositiongs:
          - name: <first class name>
            num: <number of the objects>
          - name: <second class name>
            num: <number of the objects>
        aggregations:
          - name: <first class name>
            num: <number of the objects>
            title: <the title of the connection>
          - name: <second class name>
            num: <number of the objects>
            title: <the title of the connection>
        properties:
          - <first property>
          - <...>
          - <last property>
        styles: 
          bg: orange
        
            
e.g.:
    
    - class:
        name: ﹤﹤IDefinition﹥﹥
        aggregations:
          - name: ﹤﹤IDefinition﹥﹥
            num: 0..*
            title: Children
          - name: ﹤﹤IDefinition﹥﹥
            num: 0..1
            title: Parent
          - name: Enum;DefinitionType
            num: 1
            title: Type
        bases:
          - ﹤﹤IEntity﹥﹥
        properties:
          - "Type : ﹤﹤IType﹥﹥"
          - "Length : Integer"
          
    - class:
        name: ObjectIdentifier
        bases:
          - ﹤﹤ISnmpData﹥﹥

    - class:
        name: Variable
        compositions:
          - name: ObjectIdentifier
            num: 1
          - name: ﹤﹤ISnmpData﹥﹥
            num: 1
        styles: 
          bg: orange


generates [this](http://bit.ly/ewrwKo)
    http://yuml.me/diagram/class/[﹤﹤IDefinition﹥﹥|Type : ﹤﹤IType﹥﹥;Length : Integer],[﹤﹤IEntity﹥﹥]^[﹤﹤IDefinition﹥﹥],[﹤﹤IDefinition﹥﹥]+-0..* Children>[﹤﹤IDefinition﹥﹥],[﹤﹤IDefinition﹥﹥]+-0..1 Parent>[﹤﹤IDefinition﹥﹥],[﹤﹤IDefinition﹥﹥]+-1 Type>[Enum;DefinitionType],[ObjectIdentifier],[﹤﹤ISnmpData﹥﹥]^[ObjectIdentifier],[Variable{bg:orange}],[Variable]++-1 >[ObjectIdentifier],[Variable]++-1 >[﹤﹤ISnmpData﹥﹥].
