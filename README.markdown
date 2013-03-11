A parser designed to generate [yUML](http://www.yuml.me) URLs from YAML files.

Currently only supports what I have needed it to support, as I need to add more things its support will grow.

See LICENSE for the licensing info.

Current YAML format:

    imports:
      - <first file>
      - <second file>
    
    
    classes:
      - name: <name>
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
          
      - name: <other class name>
        .
        .
        .
        
            
To view the example replace `<path>` in `Example\sharpsnmp.yaml` with the path to this directory, then copy-paste that file into the textbox on the left and hit refresh.  This should generate

    http://yuml.me/diagram/class/[﹤﹤IObjectTree﹥﹥],[﹤﹤IObjectRegistry﹥﹥],[ObjectRegistryBase],[DefaultObjectRegistry],[ReloadableObjectRegistry],[﹤﹤IDefinition﹥﹥],[Definition],[Enum;DefinitionType|Unknown;OidValueAssignment;Scalar;Table;Entry;Column],[ObjectIdentifier],[Variable],[﹤﹤ITypeAssignment﹥﹥],[ValueRange{bg:green}],[Macro],[Choice],[IntegerType;Was Integer{bg:orange}],[Sequence],[TypeAssignment],[OctetStringType{bg:green}],[BitsType{bg:green}],[﹤﹤IConstruct﹥﹥],[﹤﹤IEntity﹥﹥],[ObjectType],[Symbol{bg:orange}],[TextualConvention{bg:orange}],[MibModule{bg:orange}],[Lexer{bg:orange}],[﹤﹤IObjectTree﹥﹥]++-1>[﹤﹤IDefinition﹥﹥],[﹤﹤IObjectRegistry﹥﹥]++-1>[﹤﹤IObjectTree﹥﹥],[﹤﹤IObjectRegistry﹥﹥]^[ObjectRegistryBase],[ObjectRegistryBase]^[DefaultObjectRegistry],[ObjectRegistryBase]^[ReloadableObjectRegistry],[﹤﹤IEntity﹥﹥]^[﹤﹤IDefinition﹥﹥],[﹤﹤IDefinition﹥﹥]+-0..* Children>[﹤﹤IDefinition﹥﹥],[﹤﹤IDefinition﹥﹥]+-0..1 Parent>[﹤﹤IDefinition﹥﹥],[﹤﹤IDefinition﹥﹥]+-1 Type>[Enum;DefinitionType],[﹤﹤IDefinition﹥﹥]^[Definition],[﹤﹤ISnmpData﹥﹥]^[ObjectIdentifier],[Variable]++-1>[ObjectIdentifier],[Variable]++-1>[﹤﹤ISnmpData﹥﹥],[﹤﹤IConstruct﹥﹥]^[﹤﹤ITypeAssignment﹥﹥],[﹤﹤ITypeAssignment﹥﹥]^[Macro],[﹤﹤ITypeAssignment﹥﹥]^[Choice],[﹤﹤ITypeAssignment﹥﹥]^[IntegerType;Was Integer],[IntegerType;Was Integer]++-0..*>[ValueRange],[﹤﹤ITypeAssignment﹥﹥]^[Sequence],[﹤﹤ITypeAssignment﹥﹥]^[TypeAssignment],[﹤﹤ITypeAssignment﹥﹥]^[OctetStringType],[OctetStringType]++-0..*>[ValueRange],[﹤﹤ITypeAssignment﹥﹥]^[BitsType],[﹤﹤IConstruct﹥﹥]^[﹤﹤IEntity﹥﹥],[﹤﹤IEntity﹥﹥]^[ObjectType],[﹤﹤IConstruct﹥﹥]^[TextualConvention].
    
In the right-hand textbox and load [http://bit.ly/ev2xq3](http://bit.ly/ev2xq3) in the browser.

For some reason loading the image in the built-in browser doesn't seem to work, for now you can just copy-paste the URL to your own browser to view the image.
